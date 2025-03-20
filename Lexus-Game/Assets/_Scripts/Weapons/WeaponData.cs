using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/New Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Informations")]
    public string weaponName = "Nouvelle Arme";
    public Sprite weaponIcon;
    public int cost = 100;

    [Header("Statistiques")]
    public float damage = 20f;
    public float fireRate = 1f;
    public float range = 100f;
    public int maxAmmo = 30;
    public float reloadTime = 2f;
    public bool isAutomatic = false;

    [Header("Prefab")]
    public GameObject weaponPrefab;

    [Header("Effets")]
    public ParticleSystem muzzleFlashPrefab;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public TrailRenderer bulletTrailPrefab;
    public GameObject impactEffectPrefab;
}
