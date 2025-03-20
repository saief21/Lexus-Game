using UnityEngine;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string weaponName;
        public GameObject weaponPrefab;
        public float damage;
        public float fireRate;
        public int maxAmmo;
        public float reloadTime;
        public AudioClip shootSound;
        public ParticleSystem muzzleFlash;
    }

    [Header("Configuration")]
    [SerializeField] private List<WeaponData> availableWeapons;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private LayerMask shootableLayers;
    [SerializeField] private float weaponRange = 100f;

    private WeaponData currentWeapon;
    private int currentWeaponIndex = 0;
    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    private void Start()
    {
        if (availableWeapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    private void Update()
    {
        if (isReloading) return;

        // Changer d'arme avec la molette de la souris
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0f)
        {
            SwitchWeapon(1);
        }
        else if (scrollWheel < 0f)
        {
            SwitchWeapon(-1);
        }

        // Tirer
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            Shoot();
        }

        // Recharger
        if (Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            StartReload();
        }
    }

    private void Shoot()
    {
        nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
        currentAmmo--;

        // Effets visuels et sonores
        if (currentWeapon.muzzleFlash != null)
        {
            currentWeapon.muzzleFlash.Play();
        }

        if (currentWeapon.shootSound != null)
        {
            AudioSource.PlayClipAtPoint(currentWeapon.shootSound, transform.position);
        }

        // Raycast pour la détection des hits
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponRange, shootableLayers))
        {
            // Vérifier si on touche un ennemi
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.damage);
            }
        }
    }

    private void SwitchWeapon(int direction)
    {
        int newIndex = (currentWeaponIndex + direction + availableWeapons.Count) % availableWeapons.Count;
        EquipWeapon(newIndex);
    }

    private void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= availableWeapons.Count) return;

        currentWeaponIndex = weaponIndex;
        currentWeapon = availableWeapons[weaponIndex];

        // Détruire l'arme actuelle
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }

        // Instancier la nouvelle arme
        GameObject newWeapon = Instantiate(currentWeapon.weaponPrefab, weaponHolder);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        currentAmmo = currentWeapon.maxAmmo;
        isReloading = false;
    }

    private void StartReload()
    {
        if (isReloading || currentAmmo == currentWeapon.maxAmmo) return;

        isReloading = true;
        Invoke(nameof(FinishReload), currentWeapon.reloadTime);
    }

    private void FinishReload()
    {
        currentAmmo = currentWeapon.maxAmmo;
        isReloading = false;
    }
}
