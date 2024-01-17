using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float damages;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerMask;

    private bool playerInAttackRange;
    private bool canAttack = true;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (playerInAttackRange)
        {
            agent.speed = 0;
            if (canAttack)
            {
                canAttack = false;
                PlayerLife.instance.TakeDamages(damages);
                Invoke(nameof(ResetCanAttack), 1.5f);
            }
        }
        else
        {
            agent.speed = 5;
        }
        agent.SetDestination(PlayerLife.instance.gameObject.transform.position);

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ResetCanAttack()
    {
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        print(health);
    }
}
