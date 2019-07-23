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

    private AudioLoudnessTester loudnessTester;

    public double volumeThreshold = 0.05;

    public GameObject jaw;
    public float minYoffset = 0;
    public float maxYoffset = 0.065f;
    public float jawOpenTime = 0.2f;
    private float timer = 0;
    private float posY = 0;
    private bool isSpeaking = false;

    private SpawnAndGuide spawnAndGuide;

    public float minPitch = 1f;
    public float maxPitch = 1.5f;
    public float fadeTime = 2f;

    private bool highPitch = false;

    private float startJawOpenTime;

    public bool isCoRunning = false;

    public int currentClip = -1;
    public List<AudioClip> clips = new List<AudioClip>();
    public int maxClips = 3;

    public Text[] labels;

    private bool halted;
    private VoiceNavigation voiceNavigation;

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

        spawnAndGuide = GameObject.Find("ARSessionOrigin").GetComponent<SpawnAndGuide>();
        startJawOpenTime = jawOpenTime;

        labels = new Text[maxClips];
        for (int i = 1; i <= maxClips; i++)
        {
            string labName = string.Format("T{0}", i);
            GameObject temp = GameObject.Find(labName);
            labels[i - 1] = temp.GetComponent<Text>();
        }
        UpdateColour();
    }

    void Update ()
    {

        if (audioSource.isPlaying)
        {
            if (loudnessTester.clipLoudness > volumeThreshold && !isSpeaking)
            {
                StartCoroutine(JawMovement());
                isSpeaking = true;
                Debug.Log("Started movement");

                if (spawnAndGuide.isVertical && !highPitch)
                {
                    StartCoroutine(PitchFade(minPitch, maxPitch, fadeTime));
                    jawOpenTime = jawOpenTime / maxPitch;
                    highPitch = true;
                }
                else if (!spawnAndGuide.isVertical && highPitch)
                {
                    StartCoroutine(PitchFade(maxPitch, minPitch, fadeTime));
                    jawOpenTime = startJawOpenTime;
                    highPitch = false;
                }
            }
            else if (loudnessTester.clipLoudness < volumeThreshold && isSpeaking)
            {
                isSpeaking = false;
                Debug.Log("Stopped movement");
            }
        }
        else if (!audioSource.isPlaying && !halted)
        {
            SelectClip();
        }
        UpdateColour();
    }

    public void SelectClip()
    {
        if (currentClip <= -1) return;
        currentClip = currentClip % clips.Count + 1;
        audioSource.clip = clips[currentClip - 1];
        audioSource.Play();
    }

    public IEnumerator LoadAudioFromURL()
    {
        UpdateColour();
        if (clips.Count < maxClips)
        {
            isCoRunning = true;

            using (var www = new WWW(audioURL.text))
            {
                yield return www;
                aC = www.GetAudioClip(false, false);
                if(string.IsNullOrEmpty(www.error))
                {
                    clips.Add(aC);
                    if(clips.Count == 1)
                    {
                        currentClip = 0;
                    }
                }
                else
                {
                    Debug.Log(www.error);
                    Debug.Log("is this a valid link?");
                }
            }
            isCoRunning = false;
        }

        yield return null;
    }

    private void UpdateColour()
    {
        for (int i = 1; i <= maxClips; i++)
        {
            if (i == currentClip)
            {
                labels[i - 1].color = audioSource.isPlaying ? Color.green : Color.red;
            }
            else if (isCoRunning && clips.Count == i - 1)
            {
                labels[i - 1].color = Color.yellow;
            }
            else if (i != currentClip && clips.Count >= i)
            {
                labels[i - 1].color = Color.red;
            }
            else
            {
                labels[i - 1].color = Color.gray;
            }
        }
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
                yield return null;
            }
            else if (timer / jawOpenTime < 2 && timer / jawOpenTime >= 1)
            {
                posY = Mathf.Lerp(minYoffset, maxYoffset, timer / jawOpenTime - 1);
                yield return null;
            }
            jaw.transform.localPosition = new Vector3(0, posY, 0);
        }

        if (isSpeaking)
        {
            StartCoroutine(JawMovement());
        }
        yield return null;
    }

    IEnumerator PitchFade(float start, float end, float fadeTime)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / fadeTime;
        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / fadeTime;
            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            audioSource.pitch = currentValue;

            if (percentageComplete >= 1) break;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void VoiceControl()
    {
        voiceNavigation = GetComponent<VoiceNavigation>();
        if (voiceNavigation.isNext)
        {
            audioSource.Stop();
            SelectClip();
        }
        else if (voiceNavigation.isPause)
        {
            halted = true;
            audioSource.Pause();
        }
        else if(voiceNavigation.isStart)
        {
            audioSource.UnPause();
            halted = false;
        }
    }


}
