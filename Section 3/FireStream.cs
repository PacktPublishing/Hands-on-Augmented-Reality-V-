using UnityEngine;

public class FireStream : MonoBehaviour
{
    public GameObject assistant;
    private AssistantSpeak assistantSpeak;

    private void Awake()
    {
        assistant = GameObject.FindGameObjectWithTag("assistant");
        assistantSpeak = assistant.GetComponent<AssistantSpeak>();
    }

    public void PressedButton()
    {
        if (!assistantSpeak.isCoRunning)
        assistantSpeak.StartCoroutine(assistantSpeak.LoadAudioFromURL());
    }
}
