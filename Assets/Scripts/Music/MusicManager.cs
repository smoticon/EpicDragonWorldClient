using System.Collections;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class MusicManager : MonoBehaviour
{
    // Variables.
    public static MusicManager instance;
    private bool firstRun = true;
    private int musicFadeDuration = 4;
    private float currentVolume;

    // Audio outputs.
    private AudioSource audioSource1;
    private AudioSource audioSource2;

    // Music tracks.
    public AudioClip LoginScreen;
    public AudioClip CharacterSelection;
    public AudioClip EnterWorld;

    void Awake()
    {
        // Keep a single instance running.
        if (firstRun)
        {
            firstRun = false;
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize sources if running for the first time.
            audioSource1 = gameObject.AddComponent<AudioSource>();
            audioSource2 = gameObject.AddComponent<AudioSource>();
            audioSource1.loop = true;
            audioSource2.loop = true;
            audioSource1.volume = 0;
            audioSource2.volume = 1;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlayMusic(AudioClip audioClip)
    {
        // Crude way to avoid starting other songs on startup.
        if (Time.time < 5 && audioClip != LoginScreen)
        {
            return;
        }

        // Switch audio outputs.
        AudioSource audioSourceNext = audioSource1.isPlaying ? audioSource2 : audioSource1;
        AudioSource audioSourcePrev = audioSourceNext == audioSource1 ? audioSource2 : audioSource1;

        // Play music if not already playing.
        if (audioSourcePrev.clip != audioClip)
        {
            currentVolume = audioSourcePrev.volume;
            StartCoroutine(fadeOutAudio(audioSourcePrev));
            audioSourceNext.clip = audioClip;
            StartCoroutine(fadeInAudio(audioSourceNext));
        }
    }

    private IEnumerator fadeOutAudio(AudioSource audioSource)
    {
        float startTime = Time.time;
        while (true)
        {
            float elapsed = Time.time - startTime;
            audioSource.volume = Mathf.Clamp01(Mathf.Lerp(currentVolume, 0, elapsed / musicFadeDuration));

            // Stop playing.
            if (audioSource.volume == 0)
            {
                audioSource.Stop();
                break;
            }
            yield return null;
        }
    }

    private IEnumerator fadeInAudio(AudioSource audioSource)
    {
        // Start playing.
        audioSource.Play();

        float startTime = Time.time;
        while (true)
        {
            float elapsed = Time.time - startTime;
            audioSource.volume = Mathf.Clamp01(Mathf.Lerp(0, currentVolume, elapsed / musicFadeDuration));

            // Stop increasing volume.
            if (audioSource.volume == currentVolume)
            {
                break;
            }
            yield return null;
        }
    }
}
