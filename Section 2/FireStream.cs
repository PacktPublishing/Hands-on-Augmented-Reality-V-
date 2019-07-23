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
        assistantSpeak.StartCoroutine(assistantSpeak.LoadAudioFromURL());
    }
}
