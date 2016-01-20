using UnityEngine;
using Vuforia;

public class ControlCameraScript : MonoBehaviour {

	public void SetCameraActive(bool Active)
    {
        if (Active)
        {
            CameraDevice.Instance.Start();
        }
        else
        {
            CameraDevice.Instance.Stop();
        }
    }
}
