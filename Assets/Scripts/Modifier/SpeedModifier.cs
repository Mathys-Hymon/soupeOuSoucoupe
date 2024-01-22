using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Playermovement.instance.ModifySpeed();
            Destroy(gameObject);
        }
    }
}
