using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private Vector3 position;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, position, bulletSpeed* Time.deltaTime);

        if(transform.position == position)
        {
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
