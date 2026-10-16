
using UnityEngine;
using static XRlabToolkit.Multiplayer.Colocation.MapPreviewController;

namespace XRlabToolkit.Multiplayer.Colocation
{
   public class MapPreviewHandler : MonoBehaviour
   {
      public void MoveMapPreviewForward() => Instance.MovePreview(Vector3.forward);
      public void MoveMapPreviewBackward() => Instance.MovePreview(Vector3.back);
      public void MoveMapPreviewLeft() => Instance.MovePreview(Vector3.left);
      public void MoveMapPreviewRight() => Instance.MovePreview(Vector3.right);

      public void MoveMapPreviewUp() => Instance.MovePreview(Vector3.up);
      public void MoveMapPreviewDown() => Instance.MovePreview(Vector3.down);

      public void RotateMapPreviewLeft() => Instance.RotatePreview(-1f);
      public void RotateMapPreviewRight() => Instance.RotatePreview(1f);

      public void ResetMapPreview() => Instance.ResetPreview();

      public void VisibilityMapPreview(bool toggle) => Instance.SetPreviewVisible(toggle);
   }
}
