using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float damages;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject munitionRef;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Animator animator;

    private bool playerInAttackRange;
    private bool canAttack = true;
    private bool dead;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playSound();
    }

    private void playSound()
    {
        float pitch = Random.Range(0.8f, 1.2f);
        GetComponent<AudioSource>().pitch = pitch;
        GetComponent<AudioSource>().Play();

        float delay = Random.Range(3, 7);
        Invoke(nameof(playSound), delay);

    }

    private void Update()
    {
        if(!dead)
        {
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

            if (playerInAttackRange)
            {
                agent.speed = 0;
                animator.SetBool("isMoving", false);
                if (canAttack)
                {
                    canAttack = false;
                    animator.SetTrigger("Attack");
                    PlayerLife.instance.TakeDamages(damages);
                    Invoke(nameof(ResetCanAttack), 1.5f);
                }
            }
            else
            {
                animator.SetBool("isMoving", true);
                agent.speed = 5;
                agent.SetDestination(PlayerLife.instance.gameObject.transform.position);
            }
        }
        if (health <= 0 && !dead)
        {
            int spawnProbability = Random.Range(0, 101);
            if(spawnProbability < 20) 
            {
                Instantiate(munitionRef, transform.position, Quaternion.identity);
            }
            agent.speed = 0;
            dead = true;
            int deathAnim = Random.Range(1, 4);
            animator.SetInteger("Death", deathAnim);
            GameManager.instance.SetEnemiesRemaing();
            GameManager.instance.AddScore(10);
            Invoke(nameof(DestroyGameobject), 3f);
        }
    }

    private void DestroyGameobject()
    {
        Destroy(gameObject);
    }

    void ResetCanAttack()
    {
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
