using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARSessionOrigin))]
public class SpawnAndGuide : MonoBehaviour
{
    [SerializeField]
    GameObject assistantPrefab;
    GameObject spawnedAssistant;
    ARSessionOrigin sessionOrigin;
    static List<ARRaycastHit> planeHits = new List<ARRaycastHit>();
    Transform playerCamera;

    bool isMoving = false;
    public float speed = 1f;

    private void Awake()
    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }


    void Update ()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (spawnedAssistant == null)
                {
                    SpawnAssistantEditor(ray);
                }
                else
                {
                    if(!EventSystem.current.IsPointerOverGameObject())
                    {
                        StartCoroutine(GuideAssistantEditor(ray));
                        isMoving = true;
                    }
                }
            }
        }
        else
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);

                if (sessionOrigin.Raycast(touch.position, planeHits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = planeHits[0].pose;

                    if (spawnedAssistant == null)
                    {
                        SpawnAssistant(hitPose);
                    }
                    else
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            StartCoroutine(GuideAssistant(hitPose));
                            isMoving = true;
                        }
                    }
                }
            }
        }
		

        if (spawnedAssistant != null && !isMoving)
        {
            spawnedAssistant.transform.LookAt(playerCamera);
        }
	}

    private void SpawnAssistant(Pose hitPose)
    {
        spawnedAssistant = Instantiate(assistantPrefab, hitPose.position, hitPose.rotation);
    }

    private void SpawnAssistantEditor(Ray ray)
    {
        spawnedAssistant = Instantiate(assistantPrefab, ray.GetPoint(2), Quaternion.identity);
    }

    private IEnumerator GuideAssistant(Pose hitPose)
    {
        spawnedAssistant.transform.LookAt(hitPose.position);

        while(spawnedAssistant.transform.position != hitPose.position)
        {
            spawnedAssistant.transform.position = Vector3.MoveTowards(spawnedAssistant.transform.position, hitPose.position, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        isMoving = false;
        yield return null;
    }

    private IEnumerator GuideAssistantEditor(Ray ray)
    {
        spawnedAssistant.transform.LookAt(ray.GetPoint(2));

        while (spawnedAssistant.transform.position != ray.GetPoint(2))
        {
            spawnedAssistant.transform.position = Vector3.MoveTowards(spawnedAssistant.transform.position, ray.GetPoint(2), Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        isMoving = false;
        yield return null;
    }
}
