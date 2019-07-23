using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KKSpeech;
using System;

public class  VoiceNavigation: MonoBehaviour
{
    private SpawnAndGuide spawnAndGuide;
    private AssistantSpeak assistantSpeak;

    public Text resultText;
    public float recTime = 3f;
    private bool partialTriggered = false;

    public bool isNext;
    public bool isStart;
    public bool isPause;

    void Start()
    {
        if (SpeechRecognizer.ExistsOnDevice())
        {
            SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
            listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
            listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
            listener.onErrorDuringRecording.AddListener(OnError);
            //listener.onErrorOnStartRecording.AddListener(OnError);
            listener.onFinalResults.AddListener(OnFinalResult);
            listener.onPartialResults.AddListener(OnPartialResult);
            SpeechRecognizer.RequestAccess();
        }
        else
        {
            resultText.text = "Sorry, but this device doesn't support speech recognition";
        }
        spawnAndGuide = GameObject.Find("ARSessionOrigin").GetComponent<SpawnAndGuide>();
        assistantSpeak = GetComponent<AssistantSpeak>();
        resultText = GameObject.Find("VoiceText").GetComponent<Text>();

    }

    private void Update()
    {
        if (spawnAndGuide.isMoving && !SpeechRecognizer.IsRecording() && !partialTriggered)
            StartCoroutine(VoiceCommands());
    }

    public void OnFinalResult(string result)
    {
        if (!partialTriggered)
        {
            resultText.text = result.ToLower();
            CheckResults(result);
        }
    }

    public void OnPartialResult(string result)
    {
        //if (!partialTriggered)
        //{
            //partialTriggered = true;
            resultText.text = result.ToLower();
            CheckResults(result);
        //}
    }

    public void OnAvailabilityChange(bool available)
    {
        if (!available)
        {
            resultText.text = "Speech Recognition not available";
        }
        else
        {
            resultText.text = "Say something :-)";
        }
    }

    public void OnAuthorizationStatusFetched(AuthorizationStatus status)
    {
        switch (status)
        {
            case AuthorizationStatus.Authorized:
                break;
            default:
                resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
                break;
        }
    }


    public void OnError(string error)
    {
        Debug.LogError(error);
        resultText.text = "Something went wrong... Try again! \n [" + error + "]";
    }

    private IEnumerator VoiceCommands()
    {
        resultText.text = string.Empty;
        SpeechRecognizer.StartRecording(true);
        while (true)
        {
            if (partialTriggered) break;
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSecondsRealtime(recTime);
        SpeechRecognizer.StopIfRecording();

        while (true)
        {
            if (!spawnAndGuide.isMoving) break;
            yield return new WaitForEndOfFrame();
        }
        //resultText.text = "STOPPED moving";
        yield return new WaitForSecondsRealtime(1f); //data response arriving from the system
        partialTriggered = false;
        yield return null;
    }

    public void CheckResults(string result)
    {
        if (result.IndexOf("next") == 0)
        {
            isNext = true;
            partialTriggered = true;
        }
        else if (result.IndexOf("stop") == 0 || result.IndexOf("pause") == 0)
        {
            isPause = true;
            partialTriggered = true;
        }
        else if (result.IndexOf("play") == 0)
        {
            isStart = true;
            partialTriggered = true;
        }
        assistantSpeak.VoiceControl();
        isNext = false;
        isStart = false;
        isPause = false;

    }
}
