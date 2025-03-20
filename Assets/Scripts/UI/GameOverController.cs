using UnityEngine;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        finalScoreText.text = $"Final Score: {gameManager.score}";
        
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (gameManager.score > highScore)
        {
            highScore = gameManager.score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        
        highScoreText.text = $"High Score: {highScore}";
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
