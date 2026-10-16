
   public static class OVR_Helper
   {
      public static bool IsControllerConnected() => OVRInput.IsControllerConnected(OVRInput.Controller.RTouch) || OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);
      
   }

