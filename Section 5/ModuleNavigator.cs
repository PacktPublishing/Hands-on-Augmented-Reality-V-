using System.Collections;
using UnityEngine;
using GoogleARCore.Examples.CloudAnchors;

public class ModuleNavigator : MonoBehaviour
{

    public string receivedName;
    public bool isUIOn;
    public bool stopSpawning;
    public float swipeTime = 0.5f;
    private Touch touch;

    private ChangeState toggleScript;
    //private PlatformManager platformManager;

    private void Awake()
    {
        toggleScript = FindObjectOfType<ChangeState>();
        //platformManager = GetComponent<PlatformManager>();
    }

    private void Update()
    {
        touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            stopSpawning = false;
            StartCoroutine(CheckIfSwipe());
        }
        else if (touch.phase == TouchPhase.Moved)
            return;
    }

    public void Spawn()
    {
        if (!stopSpawning)
        {
            //platformManager.buildingBlock = receivedName;
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>().CmdAssignName(receivedName);
            toggleScript.CState();
        }
    }

    private IEnumerator CheckIfSwipe()
    {
        yield return new WaitForSecondsRealtime(swipeTime);
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Stationary)
            yield return null;
        else if (touch.phase == TouchPhase.Moved)
            stopSpawning = true;

        yield return null;
    }
}
