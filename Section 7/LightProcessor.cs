using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LightProcessor : MonoBehaviour {

    private Light lightSource;

    [SerializeField]
    public Material planeSurface;

    private GameObject lightButton;

    private void Awake()
    {
        lightSource = GameObject.FindGameObjectWithTag("light").GetComponent<Light>();
        lightButton = GameObject.FindGameObjectWithTag("lightbutton");
    }
    void Start ()
    {
        ARSubsystemManager.cameraFrameReceived += OnCameraFrameReceived;
        StartCoroutine(LightButtonTrigger());
	}

    private IEnumerator LightButtonTrigger()
    {
        yield return new WaitForSecondsRealtime(10f);
        lightButton.GetComponent<Collider>().enabled = true;
        yield return null;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        float brightnessLevel = eventArgs.lightEstimation.averageBrightness.Value;
        lightSource.intensity = brightnessLevel * 2;
        planeSurface.SetFloat("_Glossiness", brightnessLevel);
    }

}
