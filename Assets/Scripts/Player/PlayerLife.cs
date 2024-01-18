using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] float life;
    private bool dead;
    private PlayerInput playerInput;

    public static PlayerLife instance;
    void Start()
    {
        instance = this;
        dead = false;
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
    }

    private void Update()
    {
        if (life <= 0 && !dead)
        {
            Dead();
        }
    }

    private void Dead()
    {
        playerInput.enabled = false;
        dead = true;
        print("T mort");
        int bestScore = PlayerPrefs.GetInt("bestScore", 0);
        if (GameManager.instance.GetScore() > bestScore)
        {
            PlayerPrefs.SetInt("bestScore", GameManager.instance.GetScore());
            HUDManager.instance.UpdateBestScoreTxt(GameManager.instance.GetScore());
        }
        HUDManager.instance.ShowDeadScreen();
        GameManager.instance.SetFinalGameInfos();
    }

    public void TakeDamages(float damages)
    {
        if (life > damages)
        {
            life -= damages;
        }
        else
        {
            life = 0;
        }
        HUDManager.instance.UpdateLife(life);
    }

}
