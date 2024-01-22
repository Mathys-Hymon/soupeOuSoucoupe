using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private float health = 50;
 
    public void TakeDamage(float damages)
    {
        float pitch = Random.Range(0.8f, 1.1f);
        GetComponent<AudioSource>().pitch = pitch;
        GetComponent<AudioSource>().Play();
        if (health > damages)
        {
            health -= damages;
        }
        else
        {
            Destroy(gameObject);
            GameManager.instance.AddScore(1000);
        } 
    }
}
