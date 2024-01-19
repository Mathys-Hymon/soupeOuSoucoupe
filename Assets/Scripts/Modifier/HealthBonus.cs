using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : MonoBehaviour
{
    [SerializeField] private int health;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerLife.instance.getLife() < 100)
        {
            PlayerLife.instance.Addlife(health);
            Destroy(gameObject);
        }
    }
}
