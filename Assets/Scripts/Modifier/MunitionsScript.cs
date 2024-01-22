using System.Collections.Generic;
using UnityEngine;

public class MunitionsScript : MonoBehaviour
{
    [SerializeField] private bool automatic;
    [SerializeField] private int AmmoGiven;
    private List<WeaponScript> weapons;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weapons = InventoryScript.instance.getWeapons();

            for (int i = 0; i < weapons.Count; i++)
            {
                if(automatic)
                {
                    if (weapons[i].gameObject.tag == "AK47" || weapons[i].gameObject.tag == "UZI")
                    {
                        weapons[i].GetAmmo(AmmoGiven);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (weapons[i].gameObject.tag == "M107" || weapons[i].gameObject.tag == "Pistol")
                    {
                        weapons[i].GetAmmo(AmmoGiven);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
