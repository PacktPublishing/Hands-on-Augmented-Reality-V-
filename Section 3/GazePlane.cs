using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class GazePlane : MonoBehaviour
{
    ARSessionOrigin sessionOrigin;
    ARPlaneManager planeManager;
    Vector3 centre;

    static List<ARRaycastHit> planeHits = new List<ARRaycastHit>();
    public Text textOutput;

    private void Awake()
    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
        planeManager = GetComponent<ARPlaneManager>();
        centre = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
    }

    void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(centre);

        if (Application.isEditor)
        {
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point);

                if (hit.collider.transform.CompareTag("Vertical"))
                {
                    textOutput.text = "Vertical";
                }
                else if (hit.collider.transform.CompareTag("Horizontal"))
                {
                    textOutput.text = "Horizontal";
                }
            }
        }
        else
        {
            if (sessionOrigin.Raycast(ray, planeHits, TrackableType.PlaneWithinBounds))
            {
                var planeType = planeManager.TryGetPlane(planeHits[0].trackableId).boundedPlane.Alignment;
                textOutput.text = planeType.ToString();
            }
        }
	}
}
