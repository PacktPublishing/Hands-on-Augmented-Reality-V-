using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssistantSpeak : MonoBehaviour
{

    public GameObject canvas;
    public InputField audioURL;
    private AudioSource audioSource;
    public AudioClip aC;

    private bool inputGiven = false;
    private bool sourceFound = false;

    private AudioLoudnessTester loudnessTester;

    public double volumeThreshold = 0.05;

    public GameObject jaw;
    public float minYoffset = 0;
    public float maxYoffset = 0.065f;
    public float jawOpenTime = 0.2f;
    private float timer = 0;
    private float posY = 0;
    private bool isSpeaking = false;



    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(true);
        }
        audioURL = GameObject.Find("InputField").GetComponent<InputField>();
        audioSource = GetComponent<AudioSource>();
        loudnessTester = GetComponent<AudioLoudnessTester>();
        jaw = GameObject.Find("Bone004");
    }

    void Update ()
    {

        if (!inputGiven)
        {
            Debug.Log("No input yet");
        }
        else if (!sourceFound)
        {
            Debug.Log("Not ready yet");
        }
        else if (!audioSource.isPlaying && audioSource.clip.isReadyToPlay)
        {
            audioSource.Play();
        }
        else if (audioSource.isPlaying)
        {
            if (loudnessTester.clipLoudness > volumeThreshold && !isSpeaking)
            {
                StartCoroutine(JawMovement());
                isSpeaking = true;
                Debug.Log("Started movement");
            }
            else if (loudnessTester.clipLoudness < volumeThreshold && isSpeaking)
            {
                isSpeaking = false;
                Debug.Log("Stopped movement");
            }
        }
	}

    public IEnumerator LoadAudioFromURL()
    {
        inputGiven = true;

        using (var www = new WWW(audioURL.text))
        {
            yield return www;
            aC = www.GetAudioClip(false, false);
            audioSource.clip = aC;
        }

        sourceFound = true;

        yield return null;
    }

    private IEnumerator JawMovement()
    {
        timer = 0;
        float startTime = Time.time;

        while (timer / jawOpenTime < 2)
        {
            timer = Time.time - startTime;

            if (timer / jawOpenTime < 1)
            {
                posY = Mathf.Lerp(minYoffset, maxYoffset, timer / jawOpenTime);
                jaw.transform.localPosition = new Vector3(0, posY, 0);
                yield return null;
            }
            else if (timer / jawOpenTime < 2 && timer / jawOpenTime >= 1)
            {
                posY = Mathf.Lerp(minYoffset, maxYoffset, timer / jawOpenTime - 1);
                jaw.transform.localPosition = new Vector3(0, posY, 0);
                yield return null;
            }
        }

        if (isSpeaking)
        {
            StartCoroutine(JawMovement());
        }
        yield return null;
    }


}
