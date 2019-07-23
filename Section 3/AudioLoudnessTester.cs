using UnityEngine;

public class AudioLoudnessTester : MonoBehaviour
{
    private AudioSource audioSource;
    public float updateInterval = 0.1f;
    public int sampleDataLength = 1024;
    public float clipLoudness;
    private float[] clipSampleData;
    private float timer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clipSampleData = new float[sampleDataLength];
    }

    void Update ()
    {
        if (audioSource.isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                timer = 0f;
                audioSource.clip.GetData(clipSampleData, audioSource.timeSamples);
                clipLoudness = 0f;
                foreach (float sample in clipSampleData)
                {
                    clipLoudness += Mathf.Abs(sample);
                }
                clipLoudness /= sampleDataLength;
            }
        }

	}
}
