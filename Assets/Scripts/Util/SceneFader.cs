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
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @author FlatTutorials
 * @URL https://www.assetstore.unity3d.com/en/#!/content/81753
 */
public class SceneFader : MonoBehaviour
{
    [HideInInspector]
    public bool start = false;
    [HideInInspector]
    public float fadeDamp = 0.0f;
    [HideInInspector]
    public string fadeScene;
    [HideInInspector]
    public float alpha = 0.0f;
    [HideInInspector]
    public Color fadeColor;
    [HideInInspector]
    public bool isFadeIn = false;

    // Set callback.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    // Remove callback.
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    // Create a texture, color it, paint it and then fade away.
    void OnGUI()
    {
        // Fallback check.
        if (!start)
        {
            return;
        }
        // Assign the color with variable alpha.
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        // Temp texture.
        Texture2D myTex;
        myTex = new Texture2D(1, 1);
        myTex.SetPixel(0, 0, fadeColor);
        myTex.Apply();
        // Print texture.
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myTex);
        // Fade in and out control.
        if (isFadeIn)
        {
            alpha = Mathf.Lerp(alpha, -0.1f, fadeDamp * Time.deltaTime);
        }
        else
        {
            alpha = Mathf.Lerp(alpha, 1.1f, fadeDamp * Time.deltaTime);
        }
        // Load scene.
        if (alpha >= 1 && !isFadeIn)
        {
            SceneManager.LoadScene(fadeScene);
            DontDestroyOnLoad(gameObject);
        }
        else if (alpha <= 0 && isFadeIn)
        {
            Destroy(gameObject);
        }
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // We can now fade in.
        isFadeIn = true;
    }

    // Create a Fader object and assign the fade scripts and the variables.
    public static void Fade(string scene, Color col, float damp)
    {
        GameObject init = new GameObject();
        init.name = "Fader";
        init.AddComponent<SceneFader>();
        SceneFader scr = init.GetComponent<SceneFader>();
        scr.fadeDamp = damp;
        scr.fadeScene = scene;
        scr.fadeColor = col;
        scr.start = true;
    }
}
