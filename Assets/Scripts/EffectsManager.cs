using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[Serializable]
public class EffectsManager : MonoBehaviour

{

    [SerializeField] Volume volume;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private FilmGrain filmGrain;
    private LensDistortion lensDistortion;
    private ColorAdjustments colorAdjustments;
    private VisualNovelManager VisualNovelManager;


    [Header("Screen UI")]
    [SerializeField] private GameObject loseLife;
    private void Start()
    {
        loseLife.SetActive(false);
        loseLife.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
    }
    private void Awake()
    {
        if (volume != null)
        {
            volume.profile.TryGet<Vignette>(out vignette);
            volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
            volume.profile.TryGet<Bloom>(out bloom);
            volume.profile.TryGet<FilmGrain>(out filmGrain);
            volume.profile.TryGet<LensDistortion>(out lensDistortion);
            volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        }
        VisualNovelManager = GetComponent<VisualNovelManager>();
    }
    private IEnumerator FadeVignette(float targetIntensity, float duration)
    {
         // Reset the alpha of the loseLife image to 0 before starting the fade
        float startIntensity = vignette.intensity.value;
        float startChromaticAberration = chromaticAberration.intensity.value;
        float elapsedTime = 0f;

        loseLife.SetActive(true);
        StartCoroutine(FadeImage(loseLife.GetComponent<Image>(), 1f, duration));
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / duration;



            vignette.intensity.Override(
                Mathf.Lerp(startIntensity, targetIntensity, progress)
            );

            
            
            chromaticAberration.intensity.Override(
                Mathf.Lerp(startChromaticAberration, targetIntensity, progress)
            );
            
            yield return null;
        }

        vignette.intensity.Override(targetIntensity);
        chromaticAberration.intensity.Override(targetIntensity);

        yield return null;

        VisualNovelManager.RestartStory();
        RestartValues();
    }
    public void RestartValues()
    {
        loseLife.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        loseLife.SetActive(false);
        vignette.intensity.Override(0f);
        chromaticAberration.intensity.Override(0f);
    }
    public void StartVignetteFade( float duration)
    {
        StartCoroutine(FadeVignette(1, duration));
    }
    public IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        Color color = image.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, progress);

            image.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }
}
