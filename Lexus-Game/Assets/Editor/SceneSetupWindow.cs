using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class SceneSetupWindow : EditorWindow
{
    [MenuItem("Lexus Game/Scene Setup")]
    public static void ShowWindow()
    {
        GetWindow<SceneSetupWindow>("Scene Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Setup Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Setup Main Menu Scene"))
        {
            SetupMainMenuScene();
        }

        if (GUILayout.Button("Setup Game Scene"))
        {
            SetupGameScene();
        }

        if (GUILayout.Button("Create Required Prefabs"))
        {
            CreateRequiredPrefabs();
        }
    }

    private void SetupMainMenuScene()
    {
        // Créer le Canvas
        GameObject canvas = CreateCanvas();

        // Créer les panels
        GameObject mainMenuPanel = CreatePanel("MainMenuPanel", canvas.transform);
        GameObject optionsPanel = CreatePanel("OptionsPanel", canvas.transform);
        GameObject loadingScreen = CreatePanel("LoadingScreen", canvas.transform);

        // Configurer le menu principal
        CreateButton("PlayButton", mainMenuPanel.transform, "JOUER", new Vector2(0, 50));
        CreateButton("OptionsButton", mainMenuPanel.transform, "OPTIONS", new Vector2(0, -50));
        CreateButton("QuitButton", mainMenuPanel.transform, "QUITTER", new Vector2(0, -150));
        CreateText("TitleText", mainMenuPanel.transform, "LEXUS GAME", 48, new Vector2(0, 200));

        // Configurer le menu options
        CreateSlider("SoundSlider", optionsPanel.transform, "Volume", new Vector2(0, 100));
        CreateSlider("SensitivitySlider", optionsPanel.transform, "Sensibilité", new Vector2(0, 0));
        CreateButton("BackButton", optionsPanel.transform, "RETOUR", new Vector2(0, -100));

        // Configurer l'écran de chargement
        CreateLoadingScreen(loadingScreen);

        // Désactiver les panels secondaires
        optionsPanel.SetActive(false);
        loadingScreen.SetActive(false);

        // Ajouter les scripts
        canvas.AddComponent<MainMenuController>();
        
        Debug.Log("Main Menu Scene setup completed!");
    }

    private void SetupGameScene()
    {
        // Créer le terrain
        GameObject terrain = CreateTerrain();

        // Créer les points d'apparition
        CreateSpawnPoints();

        // Créer le joueur
        CreatePlayer();

        // Créer l'UI
        GameObject canvas = CreateCanvas();
        SetupGameUI(canvas);

        // Ajouter NavMesh
        Debug.Log("Please bake the NavMesh manually: Window > AI > Navigation");

        Debug.Log("Game Scene setup completed!");
    }

    private GameObject CreateCanvas()
    {
        GameObject canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvasComponent = canvas.GetComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        return canvas;
    }

    private GameObject CreatePanel(string name, Transform parent)
    {
        GameObject panel = new GameObject(name, typeof(RectTransform));
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        return panel;
    }

    private void CreateButton(string name, Transform parent, string text, Vector2 position)
    {
        GameObject button = new GameObject(name, typeof(RectTransform), typeof(Button), typeof(Image));
        button.transform.SetParent(parent, false);
        
        RectTransform rect = button.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 60);
        rect.anchoredPosition = position;

        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(button.transform, false);
        
        TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 24;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
    }

    private void CreateText(string name, Transform parent, string text, int fontSize, Vector2 position)
    {
        GameObject textObj = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(parent, false);
        
        TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = fontSize;

        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(400, 100);
        rect.anchoredPosition = position;
    }

    private void CreateSlider(string name, Transform parent, string label, Vector2 position)
    {
        GameObject sliderObj = new GameObject(name, typeof(RectTransform), typeof(Slider));
        sliderObj.transform.SetParent(parent, false);
        
        Slider slider = sliderObj.GetComponent<Slider>();
        RectTransform rect = sliderObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(300, 20);
        rect.anchoredPosition = position;

        CreateText(name + "Label", parent, label, 24, position + new Vector2(-200, 0));
        CreateText(name + "Value", parent, "100%", 24, position + new Vector2(200, 0));
    }

    private void CreateLoadingScreen(GameObject loadingScreen)
    {
        CreateText("LoadingText", loadingScreen.transform, "CHARGEMENT...", 36, new Vector2(0, 50));
        GameObject sliderObj = new GameObject("ProgressBar", typeof(RectTransform), typeof(Slider));
        sliderObj.transform.SetParent(loadingScreen.transform, false);
        
        Slider slider = sliderObj.GetComponent<Slider>();
        RectTransform rect = sliderObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(500, 20);
        rect.anchoredPosition = new Vector2(0, -50);
    }

    private GameObject CreateTerrain()
    {
        GameObject terrain = new GameObject("Terrain");
        TerrainData terrainData = new TerrainData();
        terrainData.size = new Vector3(500, 50, 500);
        Terrain terrainComponent = terrain.AddComponent<Terrain>();
        terrainComponent.terrainData = terrainData;
        terrain.AddComponent<TerrainCollider>().terrainData = terrainData;
        
        AssetDatabase.CreateAsset(terrainData, "Assets/Terrain.asset");
        
        return terrain;
    }

    private void CreateSpawnPoints()
    {
        GameObject spawnPoints = new GameObject("SpawnPoints");
        for (int i = 0; i < 8; i++)
        {
            float angle = i * Mathf.PI * 2f / 8f;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 20f;
            
            GameObject spawnPoint = new GameObject($"SpawnPoint_{i}");
            spawnPoint.transform.SetParent(spawnPoints.transform);
            spawnPoint.transform.position = position;
            spawnPoint.tag = "SpawnPoint";
        }
    }

    private void CreatePlayer()
    {
        GameObject player = new GameObject("Player", typeof(CharacterController), typeof(PlayerController), typeof(PlayerHealth));
        player.tag = "Player";

        GameObject model = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        model.transform.SetParent(player.transform);
        model.transform.localPosition = Vector3.zero;

        GameObject cameraHolder = new GameObject("CameraHolder");
        cameraHolder.transform.SetParent(player.transform);
        cameraHolder.transform.localPosition = new Vector3(0, 1.6f, 0);

        GameObject camera = new GameObject("MainCamera", typeof(Camera), typeof(AudioListener));
        camera.transform.SetParent(cameraHolder.transform);
        camera.transform.localPosition = Vector3.zero;
        camera.tag = "MainCamera";

        GameObject weaponHolder = new GameObject("WeaponHolder");
        weaponHolder.transform.SetParent(camera.transform);
        weaponHolder.transform.localPosition = new Vector3(0.2f, -0.2f, 0.4f);
    }

    private void SetupGameUI(GameObject canvas)
    {
        // HUD
        GameObject hudPanel = CreatePanel("HUDPanel", canvas.transform);
        CreateHealthBar(hudPanel.transform);
        CreateText("AmmoText", hudPanel.transform, "30/30", 24, new Vector2(-350, -460));
        CreateText("ScoreText", hudPanel.transform, "Score: 0", 24, new Vector2(400, 460));
        CreateText("WaveText", hudPanel.transform, "Vague 1", 24, new Vector2(0, 460));
        CreateText("MoneyText", hudPanel.transform, "€0", 24, new Vector2(400, 400));
        CreateText("WeaponText", hudPanel.transform, "Pistolet", 24, new Vector2(-350, -400));

        // Pause Menu
        GameObject pauseMenu = CreatePanel("PauseMenu", canvas.transform);
        CreateText("PauseTitle", pauseMenu.transform, "PAUSE", 48, new Vector2(0, 200));
        CreateButton("ResumeButton", pauseMenu.transform, "REPRENDRE", new Vector2(0, 50));
        CreateButton("OptionsButton", pauseMenu.transform, "OPTIONS", new Vector2(0, -50));
        CreateButton("MainMenuButton", pauseMenu.transform, "MENU PRINCIPAL", new Vector2(0, -150));
        pauseMenu.SetActive(false);

        // Game Over
        GameObject gameOverPanel = CreatePanel("GameOverPanel", canvas.transform);
        CreateText("GameOverTitle", gameOverPanel.transform, "GAME OVER", 48, new Vector2(0, 200));
        CreateText("FinalScoreText", gameOverPanel.transform, "Score: 0", 36, new Vector2(0, 100));
        CreateText("HighScoreText", gameOverPanel.transform, "Meilleur Score: 0", 24, new Vector2(0, 50));
        CreateButton("RestartButton", gameOverPanel.transform, "REJOUER", new Vector2(0, -50));
        CreateButton("MenuButton", gameOverPanel.transform, "MENU PRINCIPAL", new Vector2(0, -150));
        gameOverPanel.SetActive(false);

        canvas.AddComponent<HUDController>();
    }

    private void CreateHealthBar(Transform parent)
    {
        GameObject healthBar = new GameObject("HealthBar", typeof(RectTransform), typeof(Slider));
        healthBar.transform.SetParent(parent, false);
        
        Slider slider = healthBar.GetComponent<Slider>();
        RectTransform rect = healthBar.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(300, 20);
        rect.anchoredPosition = new Vector2(0, -460);
    }

    private void CreateRequiredPrefabs()
    {
        // Créer les dossiers nécessaires
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");

        // Créer les prefabs des managers
        CreateManagerPrefab("GameManager", typeof(GameManager));
        CreateManagerPrefab("EconomyManager", typeof(EconomyManager));
        CreateManagerPrefab("UIManager", typeof(UIManager));
        CreateManagerPrefab("SceneLoader", typeof(SceneLoader));

        Debug.Log("Required prefabs created!");
    }

    private void CreateManagerPrefab(string name, System.Type componentType)
    {
        GameObject manager = new GameObject(name);
        manager.AddComponent(componentType);
        
        string prefabPath = $"Assets/Prefabs/{name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(manager, prefabPath);
        DestroyImmediate(manager);
    }
}
