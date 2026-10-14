using System;
using System.Collections.Generic;
using System.IO;
using Meta.XR.MultiplayerBlocks.Shared;
using UnityEngine;
using UnityEngine.Search;
using XRlabToolkit.Multiplayer.Colocation;

public class SpatialAnchorSharing_Manager : MonoBehaviour 
   {
      [Header("Share Anchor")]
      [Tooltip("Actual prefab that you want to spawn (example map).")]
      [SearchContext("t: prefab", SearchViewFlags.GridView | SearchViewFlags.Centered | SearchViewFlags.OpenInspectorPreview)]
      [SerializeField] private GameObject sharedAnchorPrefab;
      
      [Header("References")]
      [SerializeField] private SpatialAnchorVisualizer_Handler spatialAnchorVisualizerHandler;

      private Guid _sessionGuid, _localSaveGuid;
      
      private readonly List<OVRSpatialAnchor> _listAnchors = new();
      
      private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "shared_guid.json");
      
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

      private void OnApplicationQuit() => SaveLocalShared_GUID();

      public void SharedAnchorPoint_Spawn()
      {
         if (!OVR_Helper.IsControllerConnected() || !spatialAnchorVisualizerHandler) return;
         var spawnTrans = spatialAnchorVisualizerHandler.VisualAnchorPrefab.transform;
         var createdSpacialAnchor = Instantiate(sharedAnchorPrefab, spawnTrans.position, spawnTrans.rotation);
         createdSpacialAnchor.AddComponent<OVRSpatialAnchor>();
         _listAnchors.Add(createdSpacialAnchor.GetComponent<OVRSpatialAnchor>());
      }
      
      public void SharedAnchorPoints_Share() => SharingSpacialAnchors();

      public void SharedAnchorPoints_Load() => UnboundAnchors_Load(sharedAnchorPrefab, _sessionGuid);

      public void SaveLocalShared_GUID()
      {
         var data = new GroupGuid_Data { groupGuid = _sessionGuid.ToString() };
         var json = JsonUtility.ToJson(data, true);
         File.WriteAllTextAsync(SaveFilePath, json);
      }

      public void LoadLocalShared_GUID()
      {
         if (File.Exists(SaveFilePath))
         {
            var json = File.ReadAllText(SaveFilePath);
            var data = JsonUtility.FromJson<GroupGuid_Data>(json);
            if (!Guid.TryParse(data.groupGuid, out var parsedGuid)) return;
            _localSaveGuid = parsedGuid;
            UnboundAnchors_Load(sharedAnchorPrefab, _localSaveGuid);
            Debug.Log($"{parsedGuid.ToString()}");
         }
         else
         {
            Debug.LogWarning("No GUID Saved");
         }
      }
      public void SharedAnchorPoints_EraseAll() => OVRSpatialAnchor.EraseAnchorsAsync(_listAnchors, null);
      
      private async void SharingSpacialAnchors()
      {
        await OVRSpatialAnchor.SaveAnchorsAsync(_listAnchors);
        await OVRSpatialAnchor.ShareAsync(_listAnchors, _sessionGuid);
      }
      
      private async void UnboundAnchors_Load(GameObject obj, Guid guid)
      {
         var unboundAnchorsList = new List<OVRSpatialAnchor.UnboundAnchor>();
         var unboundResult = await OVRSpatialAnchor.LoadUnboundSharedAnchorsAsync(guid, unboundAnchorsList);
         if (unboundResult.Success)
         {
            foreach (var unboundAnchor in unboundResult.Value)
            {
               unboundAnchor.LocalizeAsync().ContinueWith((success, _) =>
               {
                  if (success)
                  {
                     var spatialAnchor = Instantiate(obj).AddComponent<OVRSpatialAnchor>();
                     unboundAnchor.BindTo(spatialAnchor);
                     _listAnchors.Add(spatialAnchor.GetComponent<OVRSpatialAnchor>());
                  }
                  else
                  {
                     Debug.Log($"Localization failed for anchor {success.ToString()}");
                  }

               }, unboundAnchor);
            }
         }
         else
         {
            Debug.LogWarning($"Load failed with error {unboundResult.Status.ToString()}.");
         }
      }

      private void SessionGuid_Get(Guid groupID)
      {
         Debug.Log($"Local GUID Received: {groupID}");
         _sessionGuid = groupID;
      }
   }

   [Serializable]
   public class GroupGuid_Data
   {
      public string groupGuid;
   }


