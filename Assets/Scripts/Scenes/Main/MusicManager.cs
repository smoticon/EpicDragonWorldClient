using UnityEngine;
using UnityEngine.Audio;

/**
 * Authors: Pantelis Andrianakis, NightBR
 * Date: April 29th 2019
 */
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Mixers - Music Players")]
    public AudioSource GameMixer;
    public AudioSource LoginMixer;
    public AudioSource CharSelectMixer;
    public AudioSource CharCreationMixer;

    [Header("SnapShots - Music Players")]
    public AudioMixerSnapshot[] AudioSnapshots;

    private int savedIndex = 0;

    private void Start()
    {
        Instance = this;
    }

    // Scene verification.
    public void PlayMusic(int index)
    {
        // Just to make sure avoid out of boundaries when Main Scene is playing.
        if (index < 0)
        {
            index = 1;
        }
        // Skip character creation music change. For now it is the same.
        if (index == 3)
        {
            return;
        }
        // Only change music if track has changed.
        if (savedIndex != index)
        {
            // Reset music time.
            if (savedIndex != 1)
            {
                LoginMixer.time = 0;
            }
            if (savedIndex != 2)
            {
                CharSelectMixer.time = 0;
            }
            if (savedIndex != 3)
            {
                CharCreationMixer.time = 0;
            }
            if (savedIndex != 4)
            {
                GameMixer.time = 0;
            }

            // Fade to new music.
            if (index == 1) // Login scene.
            {
                AudioSnapshots[index].TransitionTo(0); // No fade.
            }
            else if (index == 4) // World scene.
            {
                AudioSnapshots[index].TransitionTo(6); // Fade with 6 second delay.
            }
            else
            {
                AudioSnapshots[index].TransitionTo(2); // Fade with 2 second delay.
            }
        }
        savedIndex = index;
    }

    // Mute music section.
    public void MasterMute()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void GameMusicMute()
    {
        GameMixer.mute = !GameMixer.mute;
    }

    public void LoginMusicMute()
    {
        LoginMixer.mute = !LoginMixer.mute;
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
