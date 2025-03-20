using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    [Header("Options")]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityText;
    [SerializeField] private TMP_Text soundText;

    private void Start()
    {
        // Activer le curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Charger les paramètres sauvegardés
        LoadSettings();
    }

    public void PlayGame()
    {
        SceneLoader.LoadScene("Game");
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void HideOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnSoundVolumeChanged(float value)
    {
        AudioListener.volume = value;
        soundText.text = $"Volume: {(value * 100):0}%";
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
    }

    public void OnSensitivityChanged(float value)
    {
        sensitivityText.text = $"Sensibilité: {value:0.0}";
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);

        soundSlider.value = savedVolume;
        sensitivitySlider.value = savedSensitivity;

        OnSoundVolumeChanged(savedVolume);
        OnSensitivityChanged(savedSensitivity);
    }
}
