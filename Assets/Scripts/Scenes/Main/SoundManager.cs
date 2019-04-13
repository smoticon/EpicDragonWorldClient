using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: April 13th 2019
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioClip FOOTSTEP_SOUND;

    private void Start()
    {
        Instance = this;
    }
}
