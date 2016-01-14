using UnityEngine;
using Vuforia;

public class FocusAndLight : MonoBehaviour
{

    public bool UseTorch;

    public CameraDevice.FocusMode FocusMode;

    private bool LastTorchState;

    // Use this for initialization
    void Start()
    {
        VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(SetFocusAndTorch);
        VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnPauseTorch);
    }

    public void SwitchTorchMode()
    {
        bool NewState = !LastTorchState;
        CameraDevice.Instance.SetFlashTorchMode(NewState);
        LastTorchState = NewState;
    }

    public void SetFocusMode(CameraDevice.FocusMode FocusMode)
    {
        CameraDevice.Instance.SetFocusMode(FocusMode);
    }

    private void SetFocusAndTorch()
    {
        CameraDevice.Instance.SetFocusMode(FocusMode);
        CameraDevice.Instance.SetFlashTorchMode(UseTorch);
        LastTorchState = UseTorch;
    }

    private void OnPauseTorch(bool paused)
    {
        if (!paused)
        {
            CameraDevice.Instance.SetFocusMode(FocusMode);
            CameraDevice.Instance.SetFlashTorchMode(UseTorch);
            LastTorchState = UseTorch;
        }
    }
}
