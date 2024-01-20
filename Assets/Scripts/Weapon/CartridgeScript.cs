
using UnityEngine;

public class CartridgeScript : MonoBehaviour
{

    [SerializeField] private float timeBeforeDestroy;
    
    private AudioSource asRef;

   void Start()
    {
        asRef = GetComponent<AudioSource>();
        Invoke("DestroyCartridge", timeBeforeDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float pitch = Random.Range(0.7f, 0.9f);
        asRef.pitch = pitch;
        asRef.Play();
    }
    private void DestroyCartridge()
    {
        Destroy(gameObject);
    }
}
