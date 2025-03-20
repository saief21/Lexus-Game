using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text weaponNameText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        // S'abonner aux événements
        EconomyManager.Instance.OnScoreChanged += UpdateScore;
        EconomyManager.Instance.OnMoneyChanged += UpdateMoney;

        // Cacher les menus
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void UpdateHealth(float healthPercent)
    {
        healthBar.value = healthPercent;
    }

    public void UpdateAmmo(int current, int max)
    {
        ammoText.text = $"{current}/{max}";
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateMoney(int money)
    {
        moneyText.text = $"€{money}";
    }

    public void UpdateWave(int wave)
    {
        waveText.text = $"Vague {wave}";
    }

    public void UpdateWeaponName(string weaponName)
    {
        weaponNameText.text = weaponName;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Score Final: {EconomyManager.Instance.CurrentScore}";
        highScoreText.text = $"Meilleur Score: {EconomyManager.Instance.HighScore}";
    }

    public void TogglePauseMenu()
    {
        bool isPaused = !pauseMenu.activeSelf;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadScene("Game");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadScene("MainMenu");
    }
}
