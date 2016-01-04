using UnityEngine;
using System.Collections;
using Vuforia;

public class TollScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(abc);
        VuforiaBehaviour.Instance.RegisterOnPauseCallback(xyz);
	}
	
	private void abc()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        CameraDevice.Instance.SetFlashTorchMode(true);
    }

    private void xyz(bool paused)
    {
        if (!paused)
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            CameraDevice.Instance.SetFlashTorchMode(true);
        }
    }
}
