using UnityEngine;
using UnityEditor;

public class EffectsSetupWindow : EditorWindow
{
    [MenuItem("Lexus Game/Effects Setup")]
    public static void ShowWindow()
    {
        GetWindow<EffectsSetupWindow>("Effects Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Configuration des Effets", EditorStyles.boldLabel);

        if (GUILayout.Button("Créer les Effets de Base"))
        {
            CreateBasicEffects();
        }
    }

    private void CreateBasicEffects()
    {
        // Créer les dossiers nécessaires
        CreateDirectoryIfNotExists("Assets/Prefabs/Effects");
        CreateDirectoryIfNotExists("Assets/Materials/Effects");

        // Créer les effets de base
        CreateMuzzleFlash();
        CreateBulletTrail();
        CreateBulletImpact();
        CreateBulletHole();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Effets de base créés avec succès !");
    }

    private void CreateMuzzleFlash()
    {
        // Créer le système de particules
        GameObject muzzleFlash = new GameObject("MuzzleFlash");
        ParticleSystem ps = muzzleFlash.AddComponent<ParticleSystem>();

        // Configuration de base du système de particules
        var main = ps.main;
        main.duration = 0.1f;
        main.loop = false;
        main.startLifetime = 0.1f;
        main.startSpeed = 0f;
        main.startSize = 0.5f;
        main.startColor = new Color(1f, 0.7f, 0f, 1f);

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 15) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 15f;

        // Créer le matériau
        Material material = new Material(Shader.Find("Particles/Standard Unlit"));
        material.SetColor("_Color", Color.yellow);
        AssetDatabase.CreateAsset(material, "Assets/Materials/Effects/MuzzleFlashMaterial.mat");

        // Appliquer le matériau
        var renderer = muzzleFlash.GetComponent<ParticleSystemRenderer>();
        renderer.material = material;

        // Sauvegarder le prefab
        PrefabUtility.SaveAsPrefabAsset(muzzleFlash, "Assets/Prefabs/Effects/MuzzleFlash.prefab");
        DestroyImmediate(muzzleFlash);
    }

    private void CreateBulletTrail()
    {
        GameObject bulletTrail = new GameObject("BulletTrail");
        TrailRenderer trail = bulletTrail.AddComponent<TrailRenderer>();

        // Configuration du trail
        trail.time = 0.1f;
        trail.startWidth = 0.05f;
        trail.endWidth = 0f;
        trail.minVertexDistance = 0.1f;

        // Créer le matériau
        Material material = new Material(Shader.Find("Particles/Standard Unlit"));
        material.SetColor("_Color", Color.yellow);
        AssetDatabase.CreateAsset(material, "Assets/Materials/Effects/BulletTrailMaterial.mat");

        trail.material = material;

        PrefabUtility.SaveAsPrefabAsset(bulletTrail, "Assets/Prefabs/Effects/BulletTrail.prefab");
        DestroyImmediate(bulletTrail);
    }

    private void CreateBulletImpact()
    {
        GameObject bulletImpact = new GameObject("BulletImpact");
        ParticleSystem ps = bulletImpact.AddComponent<ParticleSystem>();

        // Configuration du système de particules
        var main = ps.main;
        main.duration = 0.2f;
        main.loop = false;
        main.startLifetime = 0.2f;
        main.startSpeed = 2f;
        main.startSize = 0.1f;
        main.startColor = new Color(1f, 0.5f, 0f, 1f);

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 20) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 45f;

        // Créer le matériau
        Material material = new Material(Shader.Find("Particles/Standard Unlit"));
        material.SetColor("_Color", Color.yellow);
        AssetDatabase.CreateAsset(material, "Assets/Materials/Effects/BulletImpactMaterial.mat");

        var renderer = bulletImpact.GetComponent<ParticleSystemRenderer>();
        renderer.material = material;

        PrefabUtility.SaveAsPrefabAsset(bulletImpact, "Assets/Prefabs/Effects/BulletImpact.prefab");
        DestroyImmediate(bulletImpact);
    }

    private void CreateBulletHole()
    {
        GameObject bulletHole = GameObject.CreatePrimitive(PrimitiveType.Quad);
        bulletHole.name = "BulletHole";
        bulletHole.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // Créer le matériau
        Material material = new Material(Shader.Find("Standard"));
        material.SetColor("_Color", Color.black);
        AssetDatabase.CreateAsset(material, "Assets/Materials/Effects/BulletHoleMaterial.mat");

        bulletHole.GetComponent<MeshRenderer>().material = material;

        PrefabUtility.SaveAsPrefabAsset(bulletHole, "Assets/Prefabs/Effects/BulletHole.prefab");
        DestroyImmediate(bulletHole);
    }

    private void CreateDirectoryIfNotExists(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string parentPath = System.IO.Path.GetDirectoryName(path).Replace("\\", "/");
            string folderName = System.IO.Path.GetFileName(path);
            AssetDatabase.CreateFolder(parentPath, folderName);
        }
    }
}
