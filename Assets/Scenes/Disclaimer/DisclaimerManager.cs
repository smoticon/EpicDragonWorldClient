using System.Collections;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: December 23rd 2017
 */
public class DisclaimerManager : MonoBehaviour
{
    public float delay = 4;

    private void Start()
    {
        MusicManager.instance.PlayMusic(MusicManager.instance.LoginScreen);
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    private IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneFader.Fade("LoginScreen", Color.black, 0.5f);
    }
}
