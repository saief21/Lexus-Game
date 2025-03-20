using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponName = "Default Weapon";
    public int damage = 10;
    public float fireRate = 0.5f;
    public float range = 100f;
    public int cost = 100;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    private float nextTimeToFire = 0f;

    public void Shoot()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = firePoint.forward * bulletSpeed;
            Destroy(bullet, 3f);
        }
    }
}
