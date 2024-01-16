using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] float life;

    public static PlayerLife instance;
    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (life <= 0)
        {
            print("T mort");
        }
    }

    public void TakeDamages(float damages)
    {
        life -= damages;
    }

}
