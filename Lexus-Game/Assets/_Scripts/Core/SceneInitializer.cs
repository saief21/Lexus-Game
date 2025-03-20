using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject economyManagerPrefab;
    [SerializeField] private GameObject uiManagerPrefab;

    private void Awake()
    {
        InitializeScene();
    }

    private void InitializeScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Initialiser les managers nécessaires selon la scène
        switch (currentScene.name)
        {
            case "MainMenu":
                InitializeMainMenu();
                break;

            case "Game":
                InitializeGameScene();
                break;
        }
    }

    private void InitializeMainMenu()
    {
        // Le menu principal a besoin uniquement du UIManager
        if (FindFirstObjectByType<UIManager>() == null && uiManagerPrefab != null)
        {
            Instantiate(uiManagerPrefab);
        }
    }

    private void InitializeGameScene()
    {
        // La scène de jeu a besoin de tous les managers
        if (FindFirstObjectByType<GameManager>() == null && gameManagerPrefab != null)
        {
            Instantiate(gameManagerPrefab);
        }

        if (FindFirstObjectByType<EconomyManager>() == null && economyManagerPrefab != null)
        {
            Instantiate(economyManagerPrefab);
        }

        if (FindFirstObjectByType<UIManager>() == null && uiManagerPrefab != null)
        {
            Instantiate(uiManagerPrefab);
        }

        // Configurer les paramètres de la scène
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
