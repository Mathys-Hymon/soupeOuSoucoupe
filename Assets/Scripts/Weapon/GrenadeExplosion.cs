using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float sphereRadius;
    public void InvokeExplosion(float delay)
    {
        Invoke(nameof(Explosion), delay);
    }
    public void Explosion()
    {
        float distance = Vector3.Distance(transform.position, Playermovement.instance.transform.position);
        print(20 / distance);
        CameraShake.instance.Shake(20 / distance, 1f);
        Instantiate(explosionEffect, transform.position, transform.rotation);
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
                obj.GetComponent<PlayerLife>().TakeDamages(50);
            }
        }
        Destroy(gameObject);
    }
}
