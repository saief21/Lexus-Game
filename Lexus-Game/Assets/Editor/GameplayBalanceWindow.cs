using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameplayBalanceWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private bool showWeapons = true;
    private bool showEnemies = true;
    private bool showEconomy = true;
    private bool showDifficulty = true;

    // Difficulté globale
    private static float difficultyMultiplier = 1f;
    
    // Paramètres d'économie
    private static int startingMoney = 500;
    private static float moneyMultiplier = 1f;
    private static float scoreMultiplier = 1f;

    // Paramètres de vagues
    private static int enemiesPerWave = 5;
    private static float timeBetweenWaves = 30f;
    private static float enemySpawnInterval = 2f;
    private static float waveScalingFactor = 1.2f;

    [MenuItem("Lexus Game/Gameplay Balance")]
    public static void ShowWindow()
    {
        GetWindow<GameplayBalanceWindow>("Gameplay Balance");
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Difficulté globale
        showDifficulty = EditorGUILayout.Foldout(showDifficulty, "Difficulté Globale", true);
        if (showDifficulty)
        {
            EditorGUI.indentLevel++;
            difficultyMultiplier = EditorGUILayout.Slider("Multiplicateur de Difficulté", difficultyMultiplier, 0.5f, 2f);
            EditorGUILayout.HelpBox("Affecte les dégâts des ennemis et leur santé", MessageType.Info);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // Armes
        showWeapons = EditorGUILayout.Foldout(showWeapons, "Configuration des Armes", true);
        if (showWeapons)
        {
            EditorGUI.indentLevel++;
            DisplayWeaponSettings();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // Ennemis
        showEnemies = EditorGUILayout.Foldout(showEnemies, "Configuration des Ennemis", true);
        if (showEnemies)
        {
            EditorGUI.indentLevel++;
            DisplayEnemySettings();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // Économie et Progression
        showEconomy = EditorGUILayout.Foldout(showEconomy, "Économie et Progression", true);
        if (showEconomy)
        {
            EditorGUI.indentLevel++;
            DisplayEconomySettings();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // Boutons de sauvegarde et reset
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Sauvegarder"))
        {
            SaveSettings();
        }
        if (GUILayout.Button("Reset"))
        {
            ResetSettings();
        }
        EditorGUILayout.EndHorizontal();

        // Presets de difficulté
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Presets de Difficulté", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Facile"))
        {
            ApplyPreset(0.75f);
        }
        if (GUILayout.Button("Normal"))
        {
            ApplyPreset(1f);
        }
        if (GUILayout.Button("Difficile"))
        {
            ApplyPreset(1.5f);
        }
        if (GUILayout.Button("Extrême"))
        {
            ApplyPreset(2f);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void DisplayWeaponSettings()
    {
        // Chercher tous les WeaponData
        string[] guids = AssetDatabase.FindAssets("t:WeaponData");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            WeaponData weapon = AssetDatabase.LoadAssetAtPath<WeaponData>(path);
            
            EditorGUILayout.LabelField(weapon.weaponName, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            weapon.damage = EditorGUILayout.FloatField("Dégâts", weapon.damage);
            weapon.fireRate = EditorGUILayout.FloatField("Cadence de Tir", weapon.fireRate);
            weapon.range = EditorGUILayout.FloatField("Portée", weapon.range);
            weapon.maxAmmo = EditorGUILayout.IntField("Munitions Max", weapon.maxAmmo);
            weapon.reloadTime = EditorGUILayout.FloatField("Temps de Rechargement", weapon.reloadTime);
            weapon.cost = EditorGUILayout.IntField("Coût", weapon.cost);
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            
            EditorUtility.SetDirty(weapon);
        }
    }

    private void DisplayEnemySettings()
    {
        // Configuration des vagues
        EditorGUILayout.LabelField("Paramètres des Vagues", EditorStyles.boldLabel);
        enemiesPerWave = EditorGUILayout.IntField("Ennemis par Vague", enemiesPerWave);
        timeBetweenWaves = EditorGUILayout.FloatField("Temps entre les Vagues (s)", timeBetweenWaves);
        enemySpawnInterval = EditorGUILayout.FloatField("Intervalle d'Apparition (s)", enemySpawnInterval);
        waveScalingFactor = EditorGUILayout.FloatField("Facteur d'Échelle des Vagues", waveScalingFactor);

        EditorGUILayout.Space();

        // Configuration des types d'ennemis
        string[] enemyPrefabs = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/Prefabs/Enemies" });
        foreach (string guid in enemyPrefabs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            EnemyAI enemyAI = enemyPrefab.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                SerializedObject serializedObject = new SerializedObject(enemyAI);
                
                EditorGUILayout.LabelField(enemyPrefab.name, EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                
                SerializedProperty property = serializedObject.GetIterator();
                property.NextVisible(true); // Skip script
                
                while (property.NextVisible(false))
                {
                    EditorGUILayout.PropertyField(property);
                }
                
                if (serializedObject.hasModifiedProperties)
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(enemyPrefab);
                }
                
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }
    }

    private void DisplayEconomySettings()
    {
        startingMoney = EditorGUILayout.IntField("Argent de Départ", startingMoney);
        moneyMultiplier = EditorGUILayout.FloatField("Multiplicateur d'Argent", moneyMultiplier);
        scoreMultiplier = EditorGUILayout.FloatField("Multiplicateur de Score", scoreMultiplier);
    }

    private void SaveSettings()
    {
        // Sauvegarder les paramètres dans PlayerPrefs ou ScriptableObject
        PlayerPrefs.SetFloat("DifficultyMultiplier", difficultyMultiplier);
        PlayerPrefs.SetInt("StartingMoney", startingMoney);
        PlayerPrefs.SetFloat("MoneyMultiplier", moneyMultiplier);
        PlayerPrefs.SetFloat("ScoreMultiplier", scoreMultiplier);
        PlayerPrefs.SetInt("EnemiesPerWave", enemiesPerWave);
        PlayerPrefs.SetFloat("TimeBetweenWaves", timeBetweenWaves);
        PlayerPrefs.SetFloat("EnemySpawnInterval", enemySpawnInterval);
        PlayerPrefs.SetFloat("WaveScalingFactor", waveScalingFactor);
        
        PlayerPrefs.Save();
        
        AssetDatabase.SaveAssets();
        
        Debug.Log("Paramètres de gameplay sauvegardés !");
    }

    private void ResetSettings()
    {
        if (EditorUtility.DisplayDialog("Reset des Paramètres", 
            "Êtes-vous sûr de vouloir réinitialiser tous les paramètres ?", 
            "Oui", "Non"))
        {
            difficultyMultiplier = 1f;
            startingMoney = 500;
            moneyMultiplier = 1f;
            scoreMultiplier = 1f;
            enemiesPerWave = 5;
            timeBetweenWaves = 30f;
            enemySpawnInterval = 2f;
            waveScalingFactor = 1.2f;
            
            SaveSettings();
        }
    }

    private void ApplyPreset(float difficulty)
    {
        difficultyMultiplier = difficulty;
        
        // Ajuster l'économie en fonction de la difficulté
        moneyMultiplier = 1f / difficulty;
        scoreMultiplier = difficulty;
        
        // Ajuster les vagues en fonction de la difficulté
        enemiesPerWave = Mathf.RoundToInt(5 * difficulty);
        timeBetweenWaves = 30f / difficulty;
        enemySpawnInterval = 2f / difficulty;
        waveScalingFactor = 1.2f * difficulty;
        
        SaveSettings();
    }
}
