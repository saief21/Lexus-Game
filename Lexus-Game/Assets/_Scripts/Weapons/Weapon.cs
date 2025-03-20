using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Informations")]
    [SerializeField] private string weaponName = "Arme par défaut";
    [SerializeField] private int cost = 100;

    [Header("Statistiques")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 100f;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 2f;

    [Header("Effets")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private GameObject impactEffect;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;
    private AudioSource audioSource;
    private Camera mainCamera;

    private void Start()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            StartReload();
        }
    }

    public void Shoot()
    {
        if (isReloading || Time.time < nextFireTime || currentAmmo <= 0)
            return;

        nextFireTime = Time.time + 1f / fireRate;
        currentAmmo--;

        // Effets visuels et sonores
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);

        // Raycast pour la détection des hits
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            // Créer le trail de la balle
            if (bulletTrail != null)
            {
                TrailRenderer trail = Instantiate(bulletTrail, transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit.point));
            }

            // Effet d'impact
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }

            // Dégâts
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private System.Collections.IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(startPosition, hitPoint);
        float remainingDistance = distance;
        float trailSpeed = 200f;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= trailSpeed * Time.deltaTime;
            yield return null;
        }

        trail.transform.position = hitPoint;
        Destroy(trail.gameObject, trail.time);
    }

    private void StartReload()
    {
        if (currentAmmo == maxAmmo || isReloading)
            return;

        isReloading = true;
        if (reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    public int GetCost()
    {
        return cost;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }

    public bool IsReloading()
    {
        return isReloading;
    }
}
