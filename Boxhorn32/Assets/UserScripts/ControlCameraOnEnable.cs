using UnityEngine;
using Vuforia;

public class ControlCameraOnEnable : MonoBehaviour
{
    public void OnEnable()
    {
        if (gameObject.tag == "ScanUI" || gameObject.tag == "CameraOn")
        {
            CameraDevice.Instance.Start();
        }
        else if (gameObject.tag == "CameraOff")
        {
            CameraDevice.Instance.Stop();
        }
    }
}
