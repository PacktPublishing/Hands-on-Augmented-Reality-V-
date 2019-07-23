using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public sealed class SeeThroughController : MonoBehaviour
{

    [SerializeField]
    private Camera seeThroughCamera;

    [SerializeField]
    private Material backgroundMaterial;

    public Material BackgroundMaterial
    {
        get { return backgroundMaterial; }
        set {
            backgroundMaterial = value;

            seeThroughRenderer.Mode = UnityEngine.XR.ARRenderMode.MaterialAsBackground;
            seeThroughRenderer.BackgroundMaterial = backgroundMaterial;

            if (ARSubsystemManager.cameraSubsystem != null) {
                ARSubsystemManager.cameraSubsystem.Material = backgroundMaterial;
            }
        }
    }

    SeeThroughRenderer seeThroughRenderer;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        seeThroughRenderer = new SeeThroughRenderer(seeThroughCamera, backgroundMaterial);

        var cameraSubsystem = ARSubsystemManager.cameraSubsystem;
        if (cameraSubsystem != null) {
            cameraSubsystem.Camera = seeThroughCamera;
            cameraSubsystem.Material = BackgroundMaterial;
        }

        ARSubsystemManager.cameraFrameReceived += OnCameraFrameReceived;
    }



    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {

        seeThroughRenderer.Mode = UnityEngine.XR.ARRenderMode.MaterialAsBackground;
                                 
        if (seeThroughRenderer.BackgroundMaterial != BackgroundMaterial) {
            BackgroundMaterial = BackgroundMaterial;
        }
    }

}
