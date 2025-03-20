using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("UI References")]
    public Slider healthBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI moneyText;
    public GameObject weaponWheel;

    private GameManager gameManager;
    private PlayerController player;

    void Start()
    {
        gameManager = GameManager.Instance;
        player = FindObjectOfType<PlayerController>();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
        HandleWeaponWheel();
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {gameManager.score}";
        moneyText.text = $"${gameManager.money}";
    }

    void HandleWeaponWheel()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            weaponWheel.SetActive(!weaponWheel.activeSelf);
            Time.timeScale = weaponWheel.activeSelf ? 0f : 1f;
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth / maxHealth;
    }
}
