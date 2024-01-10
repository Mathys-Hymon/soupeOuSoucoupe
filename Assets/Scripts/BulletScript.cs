using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        Vector3 movement = transform.forward * bulletSpeed * Time.deltaTime;
        transform.position = transform.position + movement;
        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //dealt damage 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
