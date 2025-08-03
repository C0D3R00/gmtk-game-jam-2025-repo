using UnityEngine;

public class LoopSoundPlayer : MonoBehaviour
{
    public static LoopSoundPlayer Instance { get; private set; }

    [Header("Sound Clips")]
    public AudioClip oneShotClip;
    public AudioClip loopingClip;

    [Header("Audio Sources")]
    public AudioSource oneShotSource;
    public AudioSource loopingSource;

    [Header("Settings")]
    public float oneShotVolume = 1f;
    public float loopingVolume = 1f;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // optional if you want it persistent

        // Create sources if not assigned in Inspector
        if (!oneShotSource) oneShotSource = gameObject.AddComponent<AudioSource>();
        if (!loopingSource) loopingSource = gameObject.AddComponent<AudioSource>();

        oneShotSource.loop = false;
        loopingSource.loop = true;

        oneShotSource.playOnAwake = false;
        loopingSource.playOnAwake = false;
    }

    public void PlayLoopSounds()
    {
        // Play non-looping sound
        if (oneShotClip)
        {
            oneShotSource.clip = oneShotClip;
            oneShotSource.volume = oneShotVolume;
            oneShotSource.Play();
        }

        // Restart looping sound
        if (loopingClip)
        {
            loopingSource.Stop(); // restart if already playing
            loopingSource.clip = loopingClip;
            loopingSource.volume = loopingVolume;
            loopingSource.Play();
        }
    }

    public void StopLoopingSound()
    {
        loopingSource.Stop();
    }
}
