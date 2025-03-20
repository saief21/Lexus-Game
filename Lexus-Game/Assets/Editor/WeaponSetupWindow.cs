using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WeaponSetupWindow : EditorWindow
{
    private List<WeaponSetupData> weaponsToCreate = new List<WeaponSetupData>();
    private Vector2 scrollPosition;

    private class WeaponSetupData
    {
        public string name = "Nouvelle Arme";
        public GameObject modelPrefab;
        public float damage = 20f;
        public float fireRate = 1f;
        public int maxAmmo = 30;
        public float reloadTime = 2f;
        public bool isAutomatic = false;
        public int cost = 100;
    }

    [MenuItem("Lexus Game/Weapon Setup")]
    public static void ShowWindow()
    {
        GetWindow<WeaponSetupWindow>("Weapon Setup");
    }

    private void OnEnable()
    {
        // Ajouter quelques armes par défaut
        if (weaponsToCreate.Count == 0)
        {
            weaponsToCreate.Add(new WeaponSetupData
            {
                name = "Pistolet",
                damage = 20f,
                fireRate = 2f,
                maxAmmo = 12,
                reloadTime = 1.5f,
                cost = 0
            });

            weaponsToCreate.Add(new WeaponSetupData
            {
                name = "Fusil d'Assaut",
                damage = 15f,
                fireRate = 10f,
                maxAmmo = 30,
                reloadTime = 2f,
                isAutomatic = true,
                cost = 500
            });

            weaponsToCreate.Add(new WeaponSetupData
            {
                name = "Fusil de Sniper",
                damage = 100f,
                fireRate = 1f,
                maxAmmo = 5,
                reloadTime = 3f,
                cost = 1000
            });
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Configuration des Armes", EditorStyles.boldLabel);

        if (GUILayout.Button("Ajouter une Arme"))
        {
            weaponsToCreate.Add(new WeaponSetupData());
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < weaponsToCreate.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Arme {i + 1}", EditorStyles.boldLabel);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                weaponsToCreate.RemoveAt(i);
                continue;
            }
            EditorGUILayout.EndHorizontal();

            var weapon = weaponsToCreate[i];
            weapon.name = EditorGUILayout.TextField("Nom", weapon.name);
            weapon.modelPrefab = (GameObject)EditorGUILayout.ObjectField("Modèle 3D", weapon.modelPrefab, typeof(GameObject), false);
            weapon.damage = EditorGUILayout.FloatField("Dégâts", weapon.damage);
            weapon.fireRate = EditorGUILayout.FloatField("Cadence de Tir", weapon.fireRate);
            weapon.maxAmmo = EditorGUILayout.IntField("Munitions Max", weapon.maxAmmo);
            weapon.reloadTime = EditorGUILayout.FloatField("Temps de Rechargement", weapon.reloadTime);
            weapon.isAutomatic = EditorGUILayout.Toggle("Tir Automatique", weapon.isAutomatic);
            weapon.cost = EditorGUILayout.IntField("Coût", weapon.cost);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Créer les Armes"))
        {
            CreateWeapons();
        }
    }

    private void CreateWeapons()
    {
        // Créer le dossier pour les WeaponData si nécessaire
        if (!AssetDatabase.IsValidFolder("Assets/WeaponData"))
        {
            AssetDatabase.CreateFolder("Assets", "WeaponData");
        }

        // Créer le dossier pour les prefabs si nécessaire
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Weapons"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Weapons");
        }

        foreach (var weaponSetup in weaponsToCreate)
        {
            // Créer le WeaponData
            var weaponData = CreateInstance<WeaponData>();
            weaponData.weaponName = weaponSetup.name;
            weaponData.damage = weaponSetup.damage;
            weaponData.fireRate = weaponSetup.fireRate;
            weaponData.maxAmmo = weaponSetup.maxAmmo;
            weaponData.reloadTime = weaponSetup.reloadTime;
            weaponData.isAutomatic = weaponSetup.isAutomatic;
            weaponData.cost = weaponSetup.cost;

            // Créer le prefab de l'arme
            GameObject weaponObject = new GameObject(weaponSetup.name);
            
            // Ajouter le modèle 3D s'il existe
            if (weaponSetup.modelPrefab != null)
            {
                GameObject model = Instantiate(weaponSetup.modelPrefab, weaponObject.transform);
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
            }

            // Ajouter les composants nécessaires
            Weapon weaponComponent = weaponObject.AddComponent<Weapon>();
            AudioSource audioSource = weaponObject.AddComponent<AudioSource>();
            
            // Configurer l'audio source
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // Son 3D

            // Créer le prefab
            string prefabPath = $"Assets/Prefabs/Weapons/{weaponSetup.name}.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(weaponObject, prefabPath);
            DestroyImmediate(weaponObject);

            // Assigner le prefab au WeaponData
            weaponData.weaponPrefab = prefab;

            // Sauvegarder le WeaponData
            string dataPath = $"Assets/WeaponData/{weaponSetup.name}Data.asset";
            AssetDatabase.CreateAsset(weaponData, dataPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Création terminée ! {weaponsToCreate.Count} armes ont été créées.");
    }
}
