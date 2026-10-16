using System;
using UnityEngine;
using UnityEngine.Search;

namespace XRlabToolkit.Multiplayer.Colocation
{
   public class MapPreviewController : MonoBehaviour
   {
      [Header("Preview Settings")]
      [Tooltip("The object shown as a visual-only preview (not the real Anchor).")]
      [field:
         SearchContext("t:prefab",
            SearchViewFlags.GridView | SearchViewFlags.Centered | SearchViewFlags.OpenInspectorPreview)]
      [field: SerializeField]
      public GameObject PreviewMapPrefab { get; private set; }

      [Space, Tooltip("Movement step in centimeters per input.")] [SerializeField, Range(1f, 100f)]
      private float moveStepCm = 5f;

      [Space, Tooltip("Rotation step in degrees per input.")] [SerializeField, Range(0f, 360f)]
      private float rotationStepDeg = 10f;

      public static MapPreviewController Instance { get; private set; }

      private void Awake()
      {
         Instance = this;
      }

      public void MovePreview(Vector3 localDirection)
      {
         Transform t = PreviewMapPrefab.transform;

         Vector3 move = t.forward * localDirection.z + t.right * localDirection.x + t.up * localDirection.y;
         move = move.normalized * moveStepCm * Time.deltaTime;

         t.position += move;
      }

      public void RotatePreview(float direction) =>
         PreviewMapPrefab.transform.Rotate(Vector3.up, direction * rotationStepDeg, Space.Self);

      public void ResetPreview()
      {
         PreviewMapPrefab.transform.position = Vector3.zero;
         PreviewMapPrefab.transform.rotation = Quaternion.identity;
      }

      public void SetPreviewVisible(bool isVisible) => PreviewMapPrefab.SetActive(isVisible);
   }

}
