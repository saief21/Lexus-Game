using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnMoneyChanged;
    public event Action<int> OnHighScoreChanged;

    private int currentScore;
    private int currentMoney;
    private int highScore;

    public int CurrentScore => currentScore;
    public int CurrentMoney => currentMoney;
    public int HighScore => highScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);

        if (currentScore > highScore)
        {
            highScore = currentScore;
            OnHighScoreChanged?.Invoke(highScore);
            SaveHighScore();
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public bool TrySpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            OnMoneyChanged?.Invoke(currentMoney);
            return true;
        }
        return false;
    }

    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void ResetMoney()
    {
        currentMoney = 0;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
