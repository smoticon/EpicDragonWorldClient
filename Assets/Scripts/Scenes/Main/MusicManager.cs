using UnityEngine;
using UnityEngine.Audio;

/**
 * Authors: NightBR
 * Date: April 29th 2019
 */
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private readonly int musicFadeDuration = 4;
    private readonly float currentVolume;

    [Header("Mixers - Music Players")]
    public AudioSource UIMixer;
    public AudioSource GameMixer;
    public AudioSource LoginMixer;
    public AudioSource LoadingMixer;
    public AudioSource CharSelectMixer;
    public AudioSource CharCreationMixer;

    [Header("SnapShots - Music Players")]
    public AudioMixerSnapshot[] AudioSnapshots;

    [Header("SnapShots - Music Players")]
    public AudioMixerSnapshot UISnapshot;
    public AudioMixerSnapshot LoadingSnapshot;


    // Initialize Transition Variables
    public float bpm = 128;
    private float m_TransitionIn;
    private float m_TransitionOut;
    private float m_QuarterNote;

    public Canvas optionsCanvas;

    private void Start()
    {
        Instance = this;

        m_QuarterNote = 60 / bpm;
        m_TransitionIn = m_QuarterNote;
        m_TransitionOut = m_QuarterNote * 32;
    }

    // Scene Verification
    public void PlayMusic(int buildIndex)
    {
        if (buildIndex > 0)
        {
            AudioSnapshots[buildIndex].TransitionTo(m_TransitionIn); // current scene music
        }
    }

    public void PlayUIMusic(int buildIndex)
    {
        if (optionsCanvas.GetComponent<Canvas>().enabled)
        {
            UISnapshot.TransitionTo(m_TransitionIn);
        }
        else
        {
            AudioSnapshots[buildIndex].TransitionTo(m_TransitionIn);
        }
    }

    public void PlayLoadingMusic(int buildIndex, bool playLoadMusic)
    {
        if (playLoadMusic)
        {
            LoadingSnapshot.TransitionTo(m_TransitionIn);
        }
        else
        {
            AudioSnapshots[buildIndex].TransitionTo(m_TransitionIn);
        }
    }
    // Mute Music Section
    public void MasterMute()
    {
        AudioListener.pause = !AudioListener.pause;
       // MasterSnapshot.TransitionTo(m_TransitionOut);
       /*
	   UIMusicMute();
       GameMusicMute();
       LoginMusicMute();
       LoadingMusicMute();
       CharSelectMusicMute();
       CharCreationMusicMute();
       */
    }

    public void UIMusicMute()
    {
        UIMixer.mute = !UIMixer.mute;
    }

    public void GameMusicMute()
    {
        GameMixer.mute = !GameMixer.mute;
    }

    public void LoginMusicMute()
    {
        LoginMixer.mute = !LoginMixer.mute;
    }

    public void LoadingMusicMute()
    {
        LoadingMixer.mute = !LoadingMixer.mute;
    }

    public void CharSelectMusicMute()
    {
        CharSelectMixer.mute = !CharSelectMixer.mute;
    }

    public void CharCreationMusicMute()
    {
        CharCreationMixer.mute = !CharCreationMixer.mute;
    }
}
