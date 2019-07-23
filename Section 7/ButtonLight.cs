using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour {


    public bool gazedAt;
    private float gazeTime = 1.5f;
    private float timer;

    private Image fillImage;
    private float value = 0f;

    private GameObject lightSource;

    private void Awake()
    {
        fillImage = transform.Find("Radial").GetComponent<Image>();
        lightSource = GameObject.FindGameObjectWithTag("light");
    }

	

	void Update ()
    {
		if (!gazedAt)
        {
            timer = 0;
            value = 0;
            fillImage.fillAmount = value;
        }
        else if (gazedAt)
        {
            timer += Time.deltaTime;
            value = timer / gazeTime;
            fillImage.fillAmount = value;

            if (timer >= gazeTime)
            {
                gazedAt = false;
                ButtonClicked();
            }
        }
	}

    private void ButtonClicked()
    {
        Transform aimSphere = GameObject.FindGameObjectWithTag("aim").transform;
        Vector3 aimPos = new Vector3(aimSphere.position.x, aimSphere.position.y+1f, aimSphere.position.z + 2f);
        lightSource.transform.position = aimPos;
        lightSource.transform.LookAt(Camera.main.transform);
        transform.parent.gameObject.SetActive(false);
    }
}
