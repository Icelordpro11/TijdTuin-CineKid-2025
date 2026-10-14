using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XRlabToolkit.Multiplayer.Colocation
{
   
   /// <summary>
   /// Provides utility methods for saving and loading anchor UUIDs as JSON files 
   /// in the application's persistent data path.
   /// </summary>
   public static class AnchorUuidsFileStore 
   {
      private static string FilePath => Path.Combine(Application.persistentDataPath, "saved_anchors_uuids.json");

      public static void Save(List<string> uuids)
      {
         AnchorUuidsCollection collection = new AnchorUuidsCollection { anchorsUuids = uuids };
         string json = JsonUtility.ToJson(collection, true);
         File.WriteAllText(FilePath, json);
         Debug.Log($"Saved {uuids.Count} UUIDs to {FilePath}");
      }
      
      public static AnchorUuidsCollection Load()
      {
         if (!File.Exists(FilePath))
         {
            Debug.LogWarning($" No file found at path: {FilePath}");
            return null;
         }

         string json = File.ReadAllText(FilePath);
         AnchorUuidsCollection collection = JsonUtility.FromJson<AnchorUuidsCollection>(json);

         if (collection?.anchorsUuids == null)
         {
            Debug.LogWarning("Uuid list is empty ");
            return null;
         }

         Debug.Log($"Successfully loaded {collection.anchorsUuids.Count} UUID(s).");
         return collection;
      }
   }
}

