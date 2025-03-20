using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [Header("Particle Systems")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public ParticleSystem explosionEffect;
    
    [Header("Settings")]
    public float particleLifetime = 1f;

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    public void PlayHitEffect(Vector3 position, Quaternion rotation)
    {
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, position, rotation);
            Destroy(effect.gameObject, particleLifetime);
        }
    }

    public void PlayExplosion(Vector3 position)
    {
        if (explosionEffect != null)
        {
            ParticleSystem effect = Instantiate(explosionEffect, position, Quaternion.identity);
            Destroy(effect.gameObject, particleLifetime);
        }
    }
}
