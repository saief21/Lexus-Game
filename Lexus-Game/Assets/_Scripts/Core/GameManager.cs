using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameState CurrentGameState { get; private set; }
    
    [Header("Paramètres du Jeu")]
    public int currentScore = 0;
    public int currency = 0;
    public int startingCurrency = 500;

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currency = startingCurrency;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Game State Management
    public void StartGame()
    {
        CurrentGameState = GameState.Playing;
        Time.timeScale = 1f;
        currentScore = 0;
        currency = startingCurrency;
        SceneManager.LoadScene("Game");
    }

    public void PauseGame()
    {
        if (CurrentGameState == GameState.Playing)
        {
            CurrentGameState = GameState.Paused;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (CurrentGameState == GameState.Paused)
        {
            CurrentGameState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }

    public void GameOver()
    {
        CurrentGameState = GameState.GameOver;
        Time.timeScale = 0f;
        SceneManager.LoadScene("GameOver");
    }

    public void ReturnToMainMenu()
    {
        CurrentGameState = GameState.MainMenu;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        StartGame();
    }
    #endregion

    #region Score and Currency
    public void AddScore(int points)
    {
        if (CurrentGameState == GameState.Playing)
        {
            currentScore += points;
        }
    }

    public void AddCurrency(int amount)
    {
        if (CurrentGameState == GameState.Playing)
        {
            currency += amount;
        }
    }

    public bool TrySpendCurrency(int amount)
    {
        if (CurrentGameState != GameState.Playing) return false;
        
        if (currency >= amount)
        {
            currency -= amount;
            return true;
        }
        return false;
    }
    #endregion
}
