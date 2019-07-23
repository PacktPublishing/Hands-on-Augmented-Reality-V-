using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGaze : MonoBehaviour {


    public bool gazedAt;
    private float gazeTime = 1.5f;
    private float timer;

    private Image fillImage;
    private float value = 0f;

    private void Awake()
    {
        fillImage = transform.Find("Radial").GetComponent<Image>();
    }

    void Start () {
		
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
        if (gameObject.transform.CompareTag("yesbutton"))
            FindObjectOfType<CameraRaycast>().SpawnSpheres();
        transform.parent.gameObject.SetActive(false);
    }
}
