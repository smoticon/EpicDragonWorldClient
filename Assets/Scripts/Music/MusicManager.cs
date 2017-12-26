/*
 * This file is part of the Epic Dragon World project.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
using System.Collections;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private bool initializedSources = false;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    private int musicFadeDuration = 4;

    public AudioClip MusicMajesticHills;
    public AudioClip MusicSeasideNight;

    private void Initialize()
    {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
        MusicMajesticHills = Resources.Load<AudioClip>("Audio/Music/MajesticHills");
        MusicSeasideNight = Resources.Load<AudioClip>("Audio/Music/SeasideNight");
        audioSource1.loop = true;
        audioSource2.loop = true;
        audioSource1.volume = 1;
        audioSource2.volume = 0;
        initializedSources = true;
    }

    void Awake()
    {
        // Keep a single instance running.
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        if (!initializedSources)
        {
            Initialize();
        }
    }

    public void PlayMusic(AudioClip audioClip)
    {
        AudioSource audioSourceNext = audioSource1.isPlaying ? audioSource2 : audioSource1;
        AudioSource audioSourcePrev = audioSourceNext == audioSource1 ? audioSource2 : audioSource1;

        if (audioSourcePrev.clip != audioClip)
        {
            StartCoroutine(fadeAudioSourceOut(audioSourcePrev));
            audioSourceNext.volume = 0;
            audioSourceNext.clip = audioClip;
            StartCoroutine(fadeAudioSourceIn(audioSourceNext));
        }
    }

    private IEnumerator fadeAudioSourceOut(AudioSource audioSource)
    {
        float startTime = Time.time;
        float startVolume = audioSource.volume;

        while (true)
        {
            float elapsed = Time.time - startTime;
            audioSource.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, 0, elapsed / musicFadeDuration));

            if (audioSource.volume == 0)
            {
                audioSource.Stop();
                break;
            }
            yield return null;
        }
    }

    private IEnumerator fadeAudioSourceIn(AudioSource audioSource)
    {
        audioSource.Play();

        float startTime = Time.time;
        float startVolume = audioSource.volume;

        while (true)
        {
            float elapsed = Time.time - startTime;
            audioSource.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, 1, elapsed / musicFadeDuration));

            if (audioSource.volume == 1)
            {
                break;
            }
            yield return null;
        }
    }
}
