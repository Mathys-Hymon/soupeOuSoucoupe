 using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        Vector3 movement = transform.up * bulletSpeed * Time.deltaTime;
        transform.position = transform.position + movement;
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print("hit");
            other.gameObject.GetComponent<EnemyBehavior>().TakeDamage(bulletDamage);
        }
        else if (!other.gameObject.CompareTag("Bullet") && other.gameObject.GetComponent<WeaponScript>() == null && other.gameObject.GetComponent<Playermovement>() == null)
        {
            print(other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
