using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using TMPro;
using UnityEngine.EventSystems;

public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private GameObject platformObject;
    public string buildingBlock;
    private GameObject carrier = null;

    private List<DetectedPlane> detectedPlanes = new List<DetectedPlane>();
    private List<GameObject> spawnedModules = new List<GameObject>();

    [SerializeField]
    private TextMeshProUGUI feedbackText;

    private bool platformSpawned;
    private bool overlapping;
    private bool oneFinger, twoFingers;

    private void Awake()
    {
        feedbackText.text = "Scanning...";
    }

    private void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
            feedbackText.text = Session.LostTrackingReason.ToString();
        else
        {
            //Session.GetTrackables<DetectedPlane>(detectedPlanes, TrackableQueryFilter.New);
            if (detectedPlanes.Count == 1)
                feedbackText.text = "Place the platform";

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
            else if (Input.touchCount == 2)
                twoFingers = true;
            else if (Input.touchCount == 1)
                oneFinger = true;

            if (oneFinger)
            {
                Touch touch = Input.GetTouch(0);

                Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit raycastHit;
                Physics.Raycast(touchRay, out raycastHit);

                TrackableHit hit;
                TrackableHitFlags raycastFilter = TrackableHitFlags.Default;

                if (touch.phase == TouchPhase.Began)
                {
                    if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                    {
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        if (!platformSpawned)
                        {
                            Instantiate(platformObject, anchor.transform.position, anchor.transform.rotation, anchor.transform);
                            feedbackText.text = "Choose a building from panel to the left";
                            platformSpawned = true;
                        }
                        else
                        {
                            for (int i = 0; i < spawnedModules.Count; i++)
                            {
                                if (Vector3.Distance(hit.Pose.position, spawnedModules[i].transform.position) < 0.2f)
                                {
                                    if (raycastHit.transform.tag != "platform")
                                    {
                                        raycastHit.transform.parent = null;
                                        carrier = raycastHit.transform.gameObject;
                                        feedbackText.text = "Building selected";
                                    }
                                    overlapping = true;
                                    break;
                                }
                                else
                                    overlapping = false;
                            }

                            if (raycastHit.transform.tag == "platform" && !overlapping)
                            {
                                anchor = hit.Trackable.CreateAnchor(hit.Pose);
                                spawnedModules.Add(Instantiate(Resources.Load("Prefabs/" + buildingBlock) as GameObject,
                                    anchor.transform.position, anchor.transform.rotation,
                                    anchor.transform));
                                feedbackText.text = "New module spawned";
                            }
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved && carrier)
                {
                    if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                    {
                        Vector3 tempPos = hit.Pose.position;
                        carrier.transform.position = tempPos;
                        feedbackText.text = "Moving the module";
                    }
                }
                else if (touch.phase == TouchPhase.Ended && carrier)
                {
                    carrier.transform.position = new Vector3(carrier.transform.position.x, carrier.transform.position.y,
                        carrier.transform.position.z);
                    carrier.GetComponent<BoxCollider>().enabled = false;

                    if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                    {
                        if (Physics.Raycast(touchRay, out raycastHit))
                        {
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            carrier.GetComponent<BoxCollider>().enabled = true;

                            for (int i = 0; i < spawnedModules.Count; i++)
                            {
                                if (carrier == spawnedModules[i])
                                    spawnedModules[i].transform.parent = anchor.transform;
                            }
                            feedbackText.text = "Module replanted";
                        }
                        else
                        {
                            carrier.GetComponent<BoxCollider>().enabled = true;
                            if (spawnedModules.Count < 2)
                                feedbackText.text = "This is the last module";
                            else
                            {
                                for (int i = 0; i < spawnedModules.Count; i++)
                                {
                                    if (carrier == spawnedModules[i])
                                    {
                                        Destroy(spawnedModules[i]);
                                        spawnedModules.RemoveAt(i);
                                    }
                                }
                                feedbackText.text = "Module destroyed";
                            }
                        }
                    }
                    carrier = null;
                }
                oneFinger = false;
            }

            else if (twoFingers)
            {
                Touch touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(touchRay, out raycastHit) && raycastHit.transform.tag != "platform")
                    {
                        raycastHit.transform.Rotate(0f, touch.deltaPosition.x, 0f);
                        feedbackText.text = "Rotating the module";
                    }
                }
                twoFingers = false;
            }
        }
       
    }
}
