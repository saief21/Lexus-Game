using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI weaponText;
    
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject weaponShopMenu;
    
    private bool isPaused = false;
    private PlayerHealth playerHealth;
    private EnemySpawner enemySpawner;
    private WeaponManager weaponManager;

    private void Start()
    {
        // Récupérer les références une seule fois au démarrage
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        weaponManager = FindFirstObjectByType<WeaponManager>();

        if (playerHealth != null)
        {
            playerHealth.onHealthChanged.AddListener(UpdateHealthBar);
            playerHealth.onPlayerDeath.AddListener(ShowGameOver);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleWeaponShop();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (GameManager.Instance != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.currentScore}";
            currencyText.text = $"€: {GameManager.Instance.currency}";
        }

        if (enemySpawner != null)
        {
            waveText.text = $"Vague: {enemySpawner.currentWave + 1}";
        }

        if (weaponManager != null)
        {
            weaponText.text = $"Arme: {weaponManager.GetCurrentWeaponName()}";
        }
    }

    public void UpdateHealthBar(float healthPercent)
    {
        if (healthBar != null)
        {
            healthBar.value = healthPercent;
        }
    }

    public void ShowGameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }

    public void ToggleWeaponShop()
    {
        weaponShopMenu.SetActive(!weaponShopMenu.activeSelf);
        Time.timeScale = weaponShopMenu.activeSelf ? 0f : 1f;
        Cursor.lockState = weaponShopMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = weaponShopMenu.activeSelf;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartGame();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ReturnToMainMenu();
    }
}
