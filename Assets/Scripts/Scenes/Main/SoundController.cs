using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Mixer - Music Player")]
    public AudioSource UIMixer;
    public AudioSource GameMixer;
    public AudioSource LoginMixer;
    public AudioSource LoadingMixer;
    public AudioSource CharSelectMixer;

    [Header("Music")]
    public AudioClip UIMusic;
    public AudioClip GameMusic;
    public AudioClip LoginMusic;
    public AudioClip LoadingMusic;
    public AudioClip CharSelectMusic;

    public void OnGameEvent(string gameEvent)
    {

    }

    public void OnButtonClick(string buttonName)
    {

    }
}
