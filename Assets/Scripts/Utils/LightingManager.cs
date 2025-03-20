using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Light mainLight;
    public Light[] ambientLights;
    public float intensityPulseSpeed = 1f;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;

    private float[] originalIntensities;
    private float pulseTime;

    void Start()
    {
        // Configure main directional light
        if (mainLight != null)
        {
            mainLight.intensity = 1f;
            mainLight.shadows = LightShadows.Soft;
        }

        // Store original ambient light intensities
        if (ambientLights != null)
        {
            originalIntensities = new float[ambientLights.Length];
            for (int i = 0; i < ambientLights.Length; i++)
            {
                originalIntensities[i] = ambientLights[i].intensity;
            }
        }
    }

    void Update()
    {
        PulseAmbientLights();
    }

    void PulseAmbientLights()
    {
        if (ambientLights == null) return;

        pulseTime += Time.deltaTime * intensityPulseSpeed;
        float pulseValue = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(pulseTime) + 1f) / 2f);

        for (int i = 0; i < ambientLights.Length; i++)
        {
            if (ambientLights[i] != null)
            {
                ambientLights[i].intensity = originalIntensities[i] * pulseValue;
            }
        }
    }

    public void FlashLights()
    {
        foreach (Light light in ambientLights)
        {
            if (light != null)
            {
                light.intensity *= 2f;
                StartCoroutine(ResetLightIntensity(light));
            }
        }
    }

    private System.Collections.IEnumerator ResetLightIntensity(Light light)
    {
        yield return new WaitForSeconds(0.1f);
        light.intensity /= 2f;
    }
}
