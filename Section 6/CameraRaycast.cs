using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class CameraRaycast : MonoBehaviour
{

    private Vector3 centre;
    private ARSessionOrigin sessionOrigin;
    private ARPlaneManager planeManager;
    static List<ARRaycastHit> planeHits = new List<ARRaycastHit>();
    private List<Vector3> vertices = new List<Vector3>();

    private CanvasFollow canvasFollow;

    private bool triggering;
    private GameObject temp;

    private void Awake()
    {
        sessionOrigin = GameObject.FindGameObjectWithTag("origin").GetComponent<ARSessionOrigin>();
        planeManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARPlaneManager>();
        canvasFollow = GetComponent<CanvasFollow>();
    }

    void Start()
    {
        centre = new Vector3(Screen.width / 2, Screen.height / 2f, 0f);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(centre);
        RaycastHit hit;

        if (sessionOrigin.Raycast(ray, planeHits, TrackableType.PlaneWithinBounds))
        {
            var planeDetected = planeManager.TryGetPlane(planeHits[0].trackableId).boundedPlane;
            planeDetected.TryGetBoundary(vertices);
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (!triggering)
            {
                triggering = true;

                if (hit.collider.transform.CompareTag("yesbutton") || hit.collider.transform.CompareTag("nobutton"))
                {
                    canvasFollow.isLookedAt = true;
                    hit.collider.transform.GetComponent<ButtonGaze>().gazedAt = true;
                    temp = hit.collider.transform.gameObject;
                }
                else if (hit.collider.transform.CompareTag("genplane"))
                {
                    hit.collider.transform.GetComponent<PlaneGaze>().gazedAt = true;
                    temp = hit.collider.transform.gameObject;
                }
            }
        }
        else
        {
            if (triggering)
            {
                triggering = false;
                if (temp.GetComponent<ButtonGaze>())
                    temp.GetComponent<ButtonGaze>().gazedAt = false;
                else
                    temp.GetComponent<PlaneGaze>().gazedAt = false;
                canvasFollow.isLookedAt = false;
                temp = null;
            }
        }
    }

    public void SpawnSpheres()
    {
        foreach (Vector3 vertice in vertices)
            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), vertice, Quaternion.identity);
    }
}
