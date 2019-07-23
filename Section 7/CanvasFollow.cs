using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollow : MonoBehaviour
{

    private GameObject canvasHolder;
    private Canvas canvas;
    private Camera cam;


    [SerializeField]
    private float canvasDistance = 1f;
    [SerializeField]
    private float positionLerpSpeed = 2f;
    [SerializeField]
    private float rotationLerpSpeed = 5f;

    public bool isLookedAt;

    private void Awake()
    {
        canvasHolder = GameObject.FindGameObjectWithTag("canvas");
        canvas = canvasHolder.GetComponent<Canvas>();
        cam = canvas.worldCamera;
    }

    void Start()
    {

    }


    void Update()
    {

        if (!isLookedAt)
        {
            float posSpeed = Time.deltaTime * positionLerpSpeed;
            Vector3 posTo = cam.transform.position + (cam.transform.forward * canvasDistance);
            canvasHolder.transform.position = Vector3.SlerpUnclamped(canvasHolder.transform.position, posTo, posSpeed);

            float rotSpeed = Time.deltaTime * rotationLerpSpeed;
            Quaternion rotTo = Quaternion.LookRotation(canvasHolder.transform.position - cam.transform.position);
            canvasHolder.transform.rotation = Quaternion.Slerp(canvasHolder.transform.rotation, rotTo, rotSpeed);
        }


    }
}
