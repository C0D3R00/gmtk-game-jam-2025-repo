using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IntervalLoopSound : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip soundClip;
    public float volume = 1f;

    [Header("Loop Timing")]
    public float playDuration = 1f; // how long the sound plays
    public float interval = 2f;     // pause between loops

    [Header("Auto Start")]
    public bool playOnStart = true;

    private AudioSource audioSource;
    private Coroutine loopCoroutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = volume;
    }

    void Start()
    {
        if (playOnStart)
            StartLoop();
    }

    public void StartLoop()
    {
        if (loopCoroutine != null)
            StopCoroutine(loopCoroutine);

        loopCoroutine = StartCoroutine(LoopSound());
    }

    public void StopLoop()
    {
        if (loopCoroutine != null)
            StopCoroutine(loopCoroutine);

        audioSource.Stop();
    }

    private IEnumerator LoopSound()
    {
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(playDuration);
            audioSource.Stop();
            yield return new WaitForSeconds(interval);
        }
    }
}
