using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    
    private Bloom bloom;
    private AmbientOcclusion ambientOcclusion;
    private ColorGrading colorGrading;

    void Start()
    {
        // Get effects
        postProcessVolume.profile.TryGetSettings(out bloom);
        postProcessVolume.profile.TryGetSettings(out ambientOcclusion);
        postProcessVolume.profile.TryGetSettings(out colorGrading);

        // Configure bloom
        bloom.intensity.value = 1f;
        bloom.threshold.value = 1f;

        // Configure AO
        ambientOcclusion.intensity.value = 1f;
        ambientOcclusion.radius.value = 0.5f;

        // Configure color grading
        colorGrading.postExposure.value = 1f;
        colorGrading.saturation.value = 10f;
        colorGrading.contrast.value = 10f;
    }

    public void IntensifyEffects()
    {
        bloom.intensity.value = 2f;
        StartCoroutine(ResetBloomIntensity());
    }

    private System.Collections.IEnumerator ResetBloomIntensity()
    {
        yield return new WaitForSeconds(0.2f);
        bloom.intensity.value = 1f;
    }
}
