using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    private GameObject fxExplosion;
    [SerializeField] private float sphereRadius;
    public void InvokeExplosion(float delay)
    {
        Invoke(nameof(Explosion), delay);
    }
    public void Explosion()
    {
        float distance = Vector3.Distance(transform.position, Playermovement.instance.transform.position);
        CameraShake.instance.Shake(10 / distance, 1f);
        fxExplosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            if (obj.CompareTag("Enemy"))
            {
                obj.GetComponent<EnemyBehavior>().TakeDamage(100);
            }
            else if (obj.CompareTag("Player"))
            {
                obj.GetComponent<PlayerLife>().TakeDamages(110/distance);
            }
        }

        Invoke(nameof(DestroyObjects), 2f);
    }
    private void DestroyObjects()
    {
        Destroy(fxExplosion.gameObject);
        Destroy(gameObject);
    }
}
