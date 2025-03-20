using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [Header("Écran de Chargement")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private float minimumLoadTime = 0.5f;

    private static SceneLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(instance.LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Activer l'écran de chargement
        loadingScreen.SetActive(true);
        
        // Attendre une frame pour que l'écran de chargement s'affiche
        yield return null;

        // Démarrer le chargement asynchrone de la scène
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        float startTime = Time.time;
        float progress = 0;

        // Mettre à jour la barre de progression
        while (progress < 1)
        {
            progress = Mathf.Min((Time.time - startTime) / minimumLoadTime, asyncLoad.progress / 0.9f);
            
            if (progressBar != null)
                progressBar.value = progress;
            
            if (progressText != null)
                progressText.text = $"Chargement... {(progress * 100):0}%";

            yield return null;
        }

        // Activer la scène
        asyncLoad.allowSceneActivation = true;

        // Attendre que la scène soit complètement chargée
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Désactiver l'écran de chargement
        loadingScreen.SetActive(false);
    }
}
