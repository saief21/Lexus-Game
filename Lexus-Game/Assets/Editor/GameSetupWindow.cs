using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class GameSetupWindow : EditorWindow
{
    [MenuItem("Lexus Game/Complete Game Setup")]
    public static void ShowWindow()
    {
        GetWindow<GameSetupWindow>("Complete Game Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Configuration Complète du Jeu", EditorStyles.boldLabel);

        if (GUILayout.Button("1. Configurer les Scènes"))
        {
            SetupScenes();
        }

        if (GUILayout.Button("2. Créer les Managers"))
        {
            CreateManagers();
        }

        if (GUILayout.Button("3. Configurer les Armes"))
        {
            SetupWeapons();
        }

        if (GUILayout.Button("4. Créer les Effets"))
        {
            SetupEffects();
        }

        if (GUILayout.Button("5. Configurer les Ennemis"))
        {
            SetupEnemies();
        }

        if (GUILayout.Button("6. Générer le NavMesh"))
        {
            GenerateNavMesh();
        }

        if (GUILayout.Button("TOUT CONFIGURER"))
        {
            SetupComplete();
        }
    }

    private void SetupComplete()
    {
        SetupScenes();
        CreateManagers();
        SetupWeapons();
        SetupEffects();
        SetupEnemies();
        GenerateNavMesh();
        
        EditorUtility.DisplayDialog("Configuration Terminée", 
            "Le jeu a été entièrement configuré !\n\n" +
            "N'oubliez pas de :\n" +
            "1. Vérifier les références dans l'inspecteur\n" +
            "2. Ajuster les paramètres selon vos besoins\n" +
            "3. Sauvegarder les scènes", 
            "OK");
    }

    private void SetupScenes()
    {
        // Utiliser SceneSetupWindow
        SceneSetupWindow.ShowWindow();
    }

    private void CreateManagers()
    {
        // Créer les managers de base
        CreateManagerPrefab("GameManager", typeof(GameManager));
        CreateManagerPrefab("EconomyManager", typeof(EconomyManager));
        CreateManagerPrefab("UIManager", typeof(UIManager));
        CreateManagerPrefab("SceneLoader", typeof(SceneLoader));
    }

    private void SetupWeapons()
    {
        // Utiliser WeaponSetupWindow
        WeaponSetupWindow.ShowWindow();
    }

    private void SetupEffects()
    {
        // Utiliser EffectsSetupWindow
        EffectsSetupWindow.ShowWindow();
    }

    private void SetupEnemies()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Enemies"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Enemies");
        }

        // Créer différents types d'ennemis
        CreateEnemy("BasicEnemy", 100f, 10f, 1f, 2f, 10, 5);
        CreateEnemy("FastEnemy", 50f, 5f, 0.5f, 3f, 15, 10);
        CreateEnemy("TankEnemy", 200f, 20f, 2f, 1f, 30, 20);
    }

    private void CreateEnemy(string name, float health, float damage, float attackRate, float speed, int scoreValue, int moneyValue)
    {
        GameObject enemy = new GameObject(name);
        
        // Ajouter les composants de base
        enemy.AddComponent<CapsuleCollider>();
        enemy.AddComponent<NavMeshAgent>();
        enemy.AddComponent<AudioSource>();
        
        // Configurer l'IA
        EnemyAI ai = enemy.AddComponent<EnemyAI>();
        
        // Configurer la santé
        EnemyHealth health_component = enemy.AddComponent<EnemyHealth>();
        
        // Ajouter un modèle de base
        GameObject model = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        model.transform.SetParent(enemy.transform);
        model.transform.localPosition = Vector3.zero;
        
        // Sauvegarder le prefab
        string prefabPath = $"Assets/Prefabs/Enemies/{name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(enemy, prefabPath);
        DestroyImmediate(enemy);
    }

    private void GenerateNavMesh()
    {
        // Ouvrir la fenêtre de Navigation
        EditorApplication.ExecuteMenuItem("Window/AI/Navigation");
        
        // Note: La génération automatique du NavMesh nécessite des APIs non publiques
        // Il est recommandé de le faire manuellement via la fenêtre de Navigation
        EditorUtility.DisplayDialog("NavMesh", 
            "Pour générer le NavMesh :\n\n" +
            "1. Dans la fenêtre Navigation, allez dans l'onglet 'Bake'\n" +
            "2. Ajustez les paramètres si nécessaire\n" +
            "3. Cliquez sur 'Bake'", 
            "OK");
    }

    private void CreateManagerPrefab(string name, System.Type componentType)
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Managers"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Managers");
        }

        GameObject manager = new GameObject(name);
        manager.AddComponent(componentType);
        
        string prefabPath = $"Assets/Prefabs/Managers/{name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(manager, prefabPath);
        DestroyImmediate(manager);
    }
}
