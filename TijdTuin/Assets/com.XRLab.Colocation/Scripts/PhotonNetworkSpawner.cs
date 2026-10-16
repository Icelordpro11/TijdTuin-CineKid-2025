using Fusion;
using UnityEngine;


namespace XRlabToolkit.Multiplayer.Colocation
{
   public class PhotonNetworkSpawner : NetworkBehaviour
   {
      public static PhotonNetworkSpawner Instance;

      private void Awake()
      {
         Instance = this;
      }

      public GameObject SpawnPrefab(GameObject prefab)
      {
         NetworkObject spawnedObj = Runner.Spawn(prefab);
         return spawnedObj.gameObject;
      }

      public GameObject SpawnPrefab(GameObject prefab, Vector3 position, Quaternion rotation)
      {
         NetworkObject spawnedObj = Runner.Spawn(prefab, position, rotation);
         return spawnedObj.gameObject;
      }
   }
}
