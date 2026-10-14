using UnityEngine;
using UnityEngine.Search;

namespace XRlabToolkit.Multiplayer.Colocation
{
   public class SpatialAnchorVisualizer_Handler : MonoBehaviour
   {
      [Header("Visuals Settings")]
      [Tooltip("Visual-only preview (not the real Anchor).")]
      [SearchContext("t: prefab", SearchViewFlags.GridView | SearchViewFlags.Centered | SearchViewFlags.OpenInspectorPreview)]
      [SerializeField] private GameObject visualAnchorPrefabs;

      [Space] [Tooltip("How far the visual moves per step (in centimeters).")]
      [SerializeField, Range(0.1f, 1f)] private float visualMoveStep;

      [Tooltip("Max distance (meters) the visual can move away from the controller.")]
      [SerializeField, Range(1, 3)] private float maxVisualDistance = 3f;

      [Space] [Tooltip("How many degrees to rotate the visual per step.")] 
      [SerializeField, Range(5f, 20f)] private float rotationStep;

      [Header("Reference")]
      [SerializeField] private OVRCameraRig ovrCameraRig;

      public GameObject VisualAnchorPrefab { get; private set; }

      #region Public Mehtods

      public void AnchorVisual_Spawner()
      {
         Transform rightHandController = ovrCameraRig.rightControllerInHandAnchor;

         if (VisualAnchorPrefab) return;
         VisualAnchorPrefab = Instantiate(visualAnchorPrefabs, rightHandController.position, rightHandController.rotation);
         VisualAnchorPrefab.transform.SetParent(rightHandController);
      }

      public void AnchorVisual_MoveForward() => VisualAnchor_Move(+visualMoveStep);

      public void AnchorVisual_MoveBackward() => VisualAnchor_Move(-visualMoveStep);

      public void AnchorVisual_RotateLeft() => VisualAnchor_Rotate(Vector3.up, rotationStep);

      public void AnchorVisual_RotateRight() => VisualAnchor_Rotate(Vector3.up, -rotationStep);

      public void AnchorVisual_RotateForward() => VisualAnchor_Rotate(Vector3.right, rotationStep);

      public void AnchorVisual_RotateBackward() => VisualAnchor_Rotate(Vector3.right, -rotationStep);

      public void AnchorVisual_RotationReset() => VisualAnchorPrefab.transform.rotation = new Quaternion();

      public void AnchorVisual_Visibility(bool isVisible) => VisualAnchorPrefab.SetActive(isVisible);

      #endregion

      #region Private Methods

      private void VisualAnchor_Move(float step)
      {
         Transform visualAnchor = VisualAnchorPrefab.transform;
         Transform rightHandController = ovrCameraRig.rightControllerInHandAnchor;

         if (!VisualAnchorPrefab || !VisualAnchorPrefab.activeSelf) return;
         float currentRot = Vector3.Dot(rightHandController.forward, visualAnchor.position - rightHandController.position);
         float targetPos = Mathf.Clamp(currentRot + step, 0f, maxVisualDistance);
         visualAnchor.position = rightHandController.position + rightHandController.forward * targetPos;
      }

      private void VisualAnchor_Rotate(Vector3 direction, float step) => VisualAnchorPrefab.transform.Rotate(direction, step, Space.Self);

      #endregion

   }
}
