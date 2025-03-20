using UnityEngine;

public class WeaponEffects : MonoBehaviour
{
    [Header("Effets de Tir")]
    [SerializeField] private ParticleSystem muzzleFlashPrefab;
    [SerializeField] private TrailRenderer bulletTrailPrefab;
    [SerializeField] private GameObject bulletImpactPrefab;
    [SerializeField] private GameObject bulletHolePrefab;

    [Header("Sons")]
    [SerializeField] private AudioClip[] shootSounds;
    [SerializeField] private AudioClip[] reloadSounds;
    [SerializeField] private AudioClip emptySound;
    
    private AudioSource audioSource;
    private Transform weaponTip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        weaponTip = transform.Find("WeaponTip");
        if (weaponTip == null)
        {
            weaponTip = transform;
        }
    }

    public void PlayMuzzleFlash()
    {
        if (muzzleFlashPrefab != null)
        {
            ParticleSystem muzzleFlash = Instantiate(muzzleFlashPrefab, weaponTip.position, weaponTip.rotation, weaponTip);
            Destroy(muzzleFlash.gameObject, muzzleFlash.main.duration);
        }
    }

    public void CreateBulletTrail(Vector3 endPoint)
    {
        if (bulletTrailPrefab != null)
        {
            TrailRenderer trail = Instantiate(bulletTrailPrefab, weaponTip.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, endPoint));
        }
    }

    private System.Collections.IEnumerator SpawnTrail(TrailRenderer trail, Vector3 endPoint)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, endPoint, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = endPoint;
        Destroy(trail.gameObject, trail.time);
    }

    public void CreateBulletImpact(RaycastHit hit)
    {
        GameObject impactPrefab = hit.transform.CompareTag("Enemy") ? bulletImpactPrefab : bulletHolePrefab;
        
        if (impactPrefab != null)
        {
            GameObject impact = Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.SetParent(hit.transform);
            Destroy(impact, 5f);
        }
    }

    public void PlayShootSound()
    {
        if (shootSounds != null && shootSounds.Length > 0)
        {
            AudioClip randomShootSound = shootSounds[Random.Range(0, shootSounds.Length)];
            audioSource.PlayOneShot(randomShootSound);
        }
    }

    public void PlayReloadSound()
    {
        if (reloadSounds != null && reloadSounds.Length > 0)
        {
            AudioClip randomReloadSound = reloadSounds[Random.Range(0, reloadSounds.Length)];
            audioSource.PlayOneShot(randomReloadSound);
        }
    }

    public void PlayEmptySound()
    {
        if (emptySound != null)
        {
            audioSource.PlayOneShot(emptySound);
        }
    }
}
