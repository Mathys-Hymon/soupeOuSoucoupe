using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAmoBonus : MonoBehaviour
{
    [SerializeField] private int nbreGrenade;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GrenadeScript.instance.GetGrenade() < 5)
        {
            GrenadeScript.instance.AddGrenade(nbreGrenade);
            Destroy(gameObject);
        }
    }
}
