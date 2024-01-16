
using UnityEngine;

public class CartridgeScript : MonoBehaviour
{

    [SerializeField] private float timeBeforeDestroy;

   void Start()
    {
        Invoke("DestroyCartridge", timeBeforeDestroy);
    }


    private void DestroyCartridge()
    {
        Destroy(gameObject);
    }
}
