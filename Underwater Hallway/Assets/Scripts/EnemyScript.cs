using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;


public class EnemyScript : MonoBehaviour
{
   [Range(0,50)] [SerializeField] float attackRange = 5, sightRange = 20, timeBetweenAttacks =3;
    [Range(0, 20)] [SerializeField] int power;

    private NavMeshAgent thisEnemy;
    private Transform playerPos;

    Animator _animator;
    string _currentState;
    const string ENEMY_IDLE = "Enemy_Idle";
    const string ENEMY_RUN = "Enemy_Run";
    const string ENEMY_ATTACK = "Enemy_Attack";

    private bool isAttacking;
    private bool isIdling = false;

    private void Start()
    {
        thisEnemy = GetComponent<NavMeshAgent>();
        playerPos = FindObjectOfType<PlayerHealth>().transform;
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(playerPos.position, this.transform.position); // distance between playah and enemy

        if (distanceFromPlayer <= sightRange && distanceFromPlayer > attackRange && !PlayerHealth.isDead )
        {
            isAttacking = false;
            thisEnemy.isStopped = false;
            StopAllCoroutines();
            //ChangeAnimationState(ENEMY_IDLE);

            ChasePlayer();
            isIdling = false;

        }
        if (distanceFromPlayer > sightRange && !PlayerHealth.isDead && !isIdling)
        {
            Debug.Log("Enemy should idle");
            thisEnemy.isStopped = true;
            StopAllCoroutines();
            IdleMode();
        }

        if (distanceFromPlayer <= attackRange && !isAttacking && !PlayerHealth.isDead)
        {
            thisEnemy.isStopped = true; //stops enemy
            ChangeAnimationState(ENEMY_ATTACK);
            StartCoroutine(AttackPlayer());
            
        }

        if (PlayerHealth.isDead)
        {
            thisEnemy.isStopped = true;
            ChangeAnimationState(ENEMY_IDLE);
        }
    }

    private void IdleMode()
    {
        ChangeAnimationState(ENEMY_IDLE);
        isIdling = true;
    }

    private void ChasePlayer()
    {
        
        ChangeAnimationState(ENEMY_RUN);
        
        
        thisEnemy.SetDestination(playerPos.position);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        

        yield return new WaitForSeconds(timeBetweenAttacks);

        FindObjectOfType<PlayerHealth>().TakeDamage(power);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }




    ///animation mumbo jumbo
    ///
    private void ChangeAnimationState(string newState)
    {
        if (newState == _currentState)
        {
            return;
        }

        _animator.Play(newState);
        _currentState = newState;
    }

    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
