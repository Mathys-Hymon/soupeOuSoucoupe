
using UnityEngine;

public class CartridgeScript : MonoBehaviour
{

    [SerializeField] private float timeBeforeDestroy;
    [SerializeField] private AudioSource asRef;

   void Start()
    {
        Invoke("DestroyCartridge", timeBeforeDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float pitch = Random.Range(0.7f, 0.8f);
        asRef.pitch = pitch;
        asRef.Play();
    }
    private void DestroyCartridge()
    {
        Destroy(gameObject);
    }
}
