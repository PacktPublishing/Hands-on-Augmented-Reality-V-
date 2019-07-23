using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGaze : MonoBehaviour {


    public bool gazedAt;
    private float gazeTime = 1.5f;
    private float timer;
    private GameObject panelCanvas;
    private Material currentMaterial;
    private GameObject sphereAim;
    public Texture selectedTexture;

    public bool isSelected;

    void Start ()
    {
        panelCanvas = GameObject.FindGameObjectWithTag("canvas").transform.GetChild(0).gameObject;
        currentMaterial = GetComponent<MeshRenderer>().materials[0];
        sphereAim = GameObject.FindGameObjectWithTag("aim");

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
        sphereAim.GetComponent<ScaleSphere>().expandIt = false;
        isSelected = true;
        currentMaterial.mainTexture = selectedTexture;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        StartCoroutine("ReinstateLayer");
    }

    private IEnumerator ReinstateLayer()
    {
        while (isSelected)
            yield return new WaitForEndOfFrame();
        currentMaterial.mainTexture = null;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void ChangeColour(Color newColour)
    {
        currentMaterial.SetColor("_Color", newColour);
    }
}
