using UnityEngine;

/**
 * Author: Ilias Vlachos
 * Date: January 3rd 2019
 */
public class Underwater : MonoBehaviour
{
    public float waterHeight;

    private bool isUnderwater;
    private Color normalColor;
    private Color underwaterColor;

    private void Start()
    {
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.3047348f, 0.5396f, 0.6037f, 0.5019f);

        // Set both modes to avoid later latency when switch and initializing.
        SetUnderwater();
        SetNormal();
    }

    private void Update()
    {
        if ((transform.position.y < waterHeight) != isUnderwater)
        {
            isUnderwater = transform.position.y < waterHeight;
            if (isUnderwater)
			{
				SetUnderwater();
			}
            else
			{
				SetNormal();
			}
        }
    }

    private void SetNormal()
    {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogMode = FogMode.Linear;
    }

    private void SetUnderwater()
    {
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogMode = FogMode.Exponential;
    }
}