using UnityEngine;

public class ImpactScript : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(DestroyActor), 4f);
    }

private void DestroyActor()
    {
        Destroy(gameObject);
    }
}
