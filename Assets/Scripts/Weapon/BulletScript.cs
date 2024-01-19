using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private AudioSource ASRef;
    [SerializeField] private GameObject bulletImpact;
    private Vector3 position;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, position, bulletSpeed* Time.deltaTime);

        if(transform.position == position)
        {
            Instantiate(bulletImpact,transform.position, transform.rotation);
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void setTargetPos(Vector3 newPosition)
    {
        position = newPosition;
    }
}
