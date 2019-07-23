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
    public GameObject currentPlane = null;

    private GameObject sphereAim;

    //private Mesh generatedMesh;

    private void Awake()
    {
        sessionOrigin = GameObject.FindGameObjectWithTag("origin").GetComponent<ARSessionOrigin>();
        planeManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARPlaneManager>();
        canvasFollow = GetComponent<CanvasFollow>();
        sphereAim = GameObject.FindGameObjectWithTag("aim");
    }

    void Start()
    {
        centre = new Vector3(Screen.width / 2, Screen.height / 2f, 0f);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(centre);
        RaycastHit hit;

        //if (sessionOrigin.Raycast(ray, planeHits, TrackableType.PlaneWithinBounds))
        //{
        //    Pose hitPose = planeHits[0].pose;

        //    var planeDetected = planeManager.TryGetPlane(planeHits[0].trackableId).boundedPlane;
        //    planeDetected.TryGetBoundary(vertices);

        //    float sumX = 0;
        //    float sumZ = 0;

        //    for (int i = 0; i < vertices.Count; i++)
        //    {
        //        sumX += vertices[i].x;
        //        sumZ += vertices[i].z;
        //    }

        //    sumX = sumX / vertices.Count;
        //    sumZ = sumZ / vertices.Count;

        //    Vector3 centrePoint = new Vector3(sumX, hitPose.position.y, sumZ);
        //    vertices.Add(centrePoint);

        //    List<int> triangles = new List<int>();

        //    for (int i = 0; i < vertices.Count - 2; i++)
        //    {
        //        triangles.Add(i + 1);
        //        triangles.Add(vertices.Count - 1);
        //        triangles.Add(i);
        //    }

        //    triangles.Add(0);
        //    triangles.Add(vertices.Count - 1);
        //    triangles.Add(vertices.Count - 2);


        //    GetComponent<MeshFilter>().mesh = generatedMesh = new Mesh();
        //    generatedMesh.vertices = vertices.ToArray();
        //    generatedMesh.triangles = triangles.ToArray();
        //}

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
                    sphereAim.GetComponent<ScaleSphere>().expandIt = true;
                    temp = hit.collider.transform.gameObject;
                    currentPlane = temp;
                }
                else if (hit.collider.transform.CompareTag("lightbutton"))
                {
                    hit.collider.transform.GetComponent<ButtonLight>().gazedAt = true;
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
                else if (temp.GetComponent<PlaneGaze>())
                    temp.GetComponent<PlaneGaze>().gazedAt = false;
                else
                    temp.GetComponent<ButtonLight>().gazedAt = false;
                sphereAim.GetComponent<ScaleSphere>().expandIt = false;
                canvasFollow.isLookedAt = false;
                temp = null;
            }
        }
    }

    //public void SpawnSpheres()
    //{
    //    foreach (Vector3 vertice in vertices)
    //        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), vertice, Quaternion.identity);
    //}
}
