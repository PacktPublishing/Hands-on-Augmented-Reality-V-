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

    private Color buttonColour;

    private void Awake()
    {
        fillImage = transform.Find("Radial").GetComponent<Image>();
        buttonColour = GetComponent<Image>().color;
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
            FindObjectOfType<CameraRaycast>().currentPlane.GetComponent<PlaneGaze>().ChangeColour(buttonColour);
        FindObjectOfType<CameraRaycast>().currentPlane.GetComponent<PlaneGaze>().isSelected = false;
        transform.parent.gameObject.SetActive(false);
    }
}
