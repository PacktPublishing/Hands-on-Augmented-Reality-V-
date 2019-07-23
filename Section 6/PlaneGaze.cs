using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGaze : MonoBehaviour {


    public bool gazedAt;
    private float gazeTime = 1.5f;
    private float timer;
    private GameObject panelCanvas;

	void Start ()
    {
        panelCanvas = GameObject.FindGameObjectWithTag("canvas").transform.GetChild(0).gameObject;
	}
	

	void Update ()
    {
        if (!gazedAt)
            timer = 0;
        else if (gazedAt)
        {
            timer += Time.deltaTime;
            if (timer >= gazeTime)
            {
                gazedAt = false;
                ButtonClicked();
            }
        }
	}

    private void ButtonClicked()
    {
        panelCanvas.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        StartCoroutine("ReinstateLayer");
    }

    private IEnumerator ReinstateLayer()
    {
        yield return new WaitForSecondsRealtime(5f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
