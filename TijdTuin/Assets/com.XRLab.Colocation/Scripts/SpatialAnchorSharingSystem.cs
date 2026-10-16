using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meta.XR.MultiplayerBlocks.Shared;
using UnityEngine;
using UnityEngine.Search;

namespace XRlabToolkit.Multiplayer.Colocation
{
   public class SpatialAnchorSharingSystem : MonoBehaviour
   {
      [Header("Share Anchor")]
      [Tooltip("Actual prefab that you want to spawn (example the map).")]
      [SearchContext("t:prefab", SearchViewFlags.GridView | SearchViewFlags.Centered | SearchViewFlags.OpenInspectorPreview)]
      [SerializeField] private GameObject _sharedAnchorPrefab;

      private Guid _sessionGuid;
      private readonly List<OVRSpatialAnchor> _anchorsList = new();

      private void OnEnable()
      {
         LocalMatchmaking.OnSessionCreateSucceeded.AddListener(SessionGuid_Get);
         LocalMatchmaking.OnSessionDiscoverSucceeded.AddListener(SessionGuid_Get);
      }

      private void OnDisable()
      {
         LocalMatchmaking.OnSessionCreateSucceeded.RemoveListener(SessionGuid_Get);
         LocalMatchmaking.OnSessionDiscoverSucceeded.RemoveListener(SessionGuid_Get);
      }
      
      public void SpawnSharedAnchor()
      {
         if (!OVR_Helper.IsControllerConnected()) return;
         
         Transform visual = MapPreviewController.Instance.PreviewMapPrefab.transform;

         if (!visual) return;
         OVRSpatialAnchor anchor = SpawnNetworkedOVRSpatialAnchor(_sharedAnchorPrefab, visual.position, visual.rotation);
         _anchorsList.Add(anchor);
      }

      public void ShareSessionAnchors() => _ = SaveAndShareAsync();

      public void LoadSharedAnchorsFormSession() => _ = LoadSharedAnchorsAsync();

      public void SaveLocalSharedAnchors()
      {
          OVRSpatialAnchor.SaveAnchorsAsync(_anchorsList);
          AnchorUuidsFileStore.Save(ExtractAnchorGuids(_anchorsList));
      }

      public void LoadLocalSharedAnchors()
      {
         AnchorUuidsCollection collection = AnchorUuidsFileStore.Load();
         if (IsDataEmpty(collection))
         {
            Debug.LogWarning("No Anchors Uuids saved locally.");
            return;
         }

         Debug.Log($"Found JSON with {collection.anchorsUuids.Count} UUID");
         _ = LoadAnchorsFromGuidsAsync(collection);
      }
      
      public void EraseLastSharedAnchor()
      {
         if (_anchorsList.Count == 0) return;
         
         OVRSpatialAnchor lastAnchor = _anchorsList[^1];

         if (!lastAnchor) return;
         _ = lastAnchor.EraseAnchorAsync();
         Destroy(lastAnchor.gameObject);
         _anchorsList.RemoveAt(_anchorsList.Count - 1);
      }
      
      private async Task SaveAndShareAsync()
      {
         if (_anchorsList.Count == 0)
         {
            Debug.LogWarning($"The list is empty: {_anchorsList.Count} ");
            return;
         }

         await OVRSpatialAnchor.SaveAnchorsAsync(_anchorsList);
         await OVRSpatialAnchor.ShareAsync(_anchorsList, _sessionGuid);
         
         Debug.Log($"Successfully saved and shared {_anchorsList.Count} anchors");
      }

      /// <summary>
      /// Asynchronously loads shared spatial anchors for the current session,
      /// </summary>
      private async Task LoadSharedAnchorsAsync()
      {
         List<OVRSpatialAnchor.UnboundAnchor> unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
         OVRResult<List<OVRSpatialAnchor.UnboundAnchor>, OVRSpatialAnchor.OperationResult> loadResult = await OVRSpatialAnchor.LoadUnboundSharedAnchorsAsync(_sessionGuid, unboundAnchors);
         
         if (!loadResult.Success)
         {
            Debug.LogWarning($"Load failed with error {loadResult.Status}");
            return;
         }

         List<OVRSpatialAnchor> anchors = await BindAndCreateAnchorsAsync(loadResult.Value, new GameObject(), SpawnMode.Local);
         _anchorsList.AddRange(anchors);
      }

      /// <summary>
      /// Loads anchors from saved UUIDs in JSON, binds them, and spawns networked prefabs.
      /// </summary>
      private async Task LoadAnchorsFromGuidsAsync(AnchorUuidsCollection collection)
      {
         List<Guid> guids = ParseGuids(collection.anchorsUuids);
         List<OVRSpatialAnchor.UnboundAnchor> unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
         OVRResult<List<OVRSpatialAnchor.UnboundAnchor>, OVRAnchor.FetchResult> loadResult = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(guids, unboundAnchors);
         
         if (!loadResult.Success)
         {
            Debug.LogWarning($"Load by UUIDs failed: {loadResult.Status}");
            return;
         }
         
         List<OVRSpatialAnchor> ovrSpatialAnchors = await BindAndCreateAnchorsAsync(loadResult.Value, _sharedAnchorPrefab, SpawnMode.Networked);
         _anchorsList.AddRange(ovrSpatialAnchors);
      }
      
      /// <summary>
      /// Localizes unbound anchors, spawns prefabs at their poses, and binds them as spatial anchors.
      /// </summary>
      private async Task<List<OVRSpatialAnchor>> BindAndCreateAnchorsAsync(List<OVRSpatialAnchor.UnboundAnchor> unboundList, GameObject prefab, SpawnMode spawnMode)
      {
         Debug.Log($"binding list count {unboundList.Count}");
         if (!prefab)
         {
            Debug.LogWarning(" prefab is null.");
            return new List<OVRSpatialAnchor>();
         }

         Func<GameObject, Vector3, Quaternion, OVRSpatialAnchor> spawnOVRAnchor = spawnMode switch
         {
            SpawnMode.Local => SpawnLocalOVRSpatialAnchor,
            SpawnMode.Networked => SpawnNetworkedOVRSpatialAnchor,
            _ => throw new ArgumentOutOfRangeException(nameof(spawnMode), spawnMode, null)
         };

         List<OVRSpatialAnchor> anchors = new(unboundList.Count);
        
         foreach (var unbound in unboundList)
         {
            if (!await unbound.LocalizeAsync())
            {
               Debug.LogWarning($"LocalizeAsync failed for {unbound.Uuid}");
               continue;
            }

            if (!unbound.TryGetPose(out Pose pose))
            {
               Debug.LogWarning($"TryGetPose failed for {unbound.Uuid}");
               continue;
            }

            Debug.Log($"Trying to spawn the anchor");
            OVRSpatialAnchor anchor = spawnOVRAnchor(prefab, pose.position, pose.rotation);
            unbound.BindTo(anchor);
            anchors.Add(anchor);
         }

         return anchors;
      }

      private void SessionGuid_Get(Guid groupID)
      {
         _sessionGuid = groupID;
         Debug.Log($"Session GUID Received: {groupID}");
      }
      
      private bool IsDataEmpty(AnchorUuidsCollection collection) => collection?.anchorsUuids == null || collection.anchorsUuids.Count == 0;
      
      private OVRSpatialAnchor SpawnNetworkedOVRSpatialAnchor(GameObject prefab, Vector3 pos, Quaternion rot)
      {
         GameObject spawnPrefab = Instantiate(prefab, pos, rot);

         if (!spawnPrefab)
         {
            Debug.LogWarning("Failed to spawn object from PhotonNetworkSpawner.");
            return null;
         }
         
         return EnsureOVRAnchor(spawnPrefab);
      }
      
      private OVRSpatialAnchor SpawnLocalOVRSpatialAnchor(GameObject prefab, Vector3 pos, Quaternion rot)
      {
         Debug.Log($"Instantiating object");
         GameObject spawnPrefab = Instantiate(prefab, pos, rot);

            if (!spawnPrefab)
         {
            Debug.LogWarning("Failed to spawn object from PhotonNetworkSpawner.");
            return null;
         }

         return EnsureOVRAnchor(spawnPrefab);
      }
      
      /// <summary>
      /// Ensures the given GameObject has an OVRSpatialAnchor, adding one if missing.
      /// </summary>
      private OVRSpatialAnchor EnsureOVRAnchor(GameObject obj) => obj.TryGetComponent(out OVRSpatialAnchor anchor) ? anchor : obj.AddComponent<OVRSpatialAnchor>();

      private List<string> ExtractAnchorGuids(List<OVRSpatialAnchor> anchors)
      {
         List<string> list = new List<string>(anchors.Count);
         for (int i = 0; i < anchors.Count; i++)
         {
            OVRSpatialAnchor a = anchors[i];
            if (a && a.Uuid != Guid.Empty) list.Add(a.Uuid.ToString());
         }
         return list;
      }

      private List<Guid> ParseGuids(List<string> uuidStrings)
      {
         List<Guid> result = new List<Guid>();
         foreach (string s in uuidStrings)
         {
            if (Guid.TryParse(s, out Guid g)) result.Add(g);
         }
         return result;
      }
   }
   public enum SpawnMode { Local, Networked }
}
