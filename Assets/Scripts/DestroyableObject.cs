using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private float health = 50;


    public void TakeDamage(float damages)
    {
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
