using UnityEngine;
using Vuforia;

public class FocusAndLight : MonoBehaviour
{

    public bool useTorch;

    public CameraDevice.FocusMode focusMode;

    // Use this for initialization
    void Start()
    {
        VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(abc);
        VuforiaBehaviour.Instance.RegisterOnPauseCallback(xyz);
    }

    private void abc()
    {
        CameraDevice.Instance.SetFocusMode(focusMode);
        CameraDevice.Instance.SetFlashTorchMode(useTorch);
    }

    private void xyz(bool paused)
    {
        if (!paused)
        {
            CameraDevice.Instance.SetFocusMode(focusMode);
            CameraDevice.Instance.SetFlashTorchMode(useTorch);
        }
    }
}
