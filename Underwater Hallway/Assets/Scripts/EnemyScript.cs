using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;


public class EnemyScript : MonoBehaviour
{
    //Variables for stats
    [Range(0,50)] [SerializeField] float attackRange = 5, sightRange = 20, timeBetweenAttacks = 3;
    [Range(0, 20)] [SerializeField] int power;

    private NavMeshAgent thisEnemy;
    private Transform playerPos;

    //Variables for animations
    Animator _animator;
    string _currentState;
    const string ENEMY_IDLE = "Enemy_Idle";
    const string ENEMY_RUN = "Enemy_Run";
    const string ENEMY_ATTACK = "Enemy_Attack";

    private bool isAttacking;
    private bool isIdling = false;

    //Variables for stun mechanic
    [SerializeField] public ParticleSystem stunParticles;
    public bool isStunned = false;
    public bool stunCooldown = false;
    public float cooldownRemaining = 10f;
    public float stunRemaining = 4.5f;
    public bool cheatOn = false; 
    //[SerializeField] private LayerMask stunLayer;
    //[SerializeField] private GameObject stunBox;


    private void Start()
    {
        thisEnemy = GetComponent<NavMeshAgent>();
        playerPos = FindObjectOfType<PlayerHealth>().transform;
        _animator = gameObject.GetComponent<Animator>();

    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(playerPos.position, this.transform.position); // distance between playah and enemy

            if ((distanceFromPlayer <= sightRange && distanceFromPlayer > attackRange && !PlayerHealth.isDead) && (!isStunned))
        {
            isAttacking = false;
            thisEnemy.isStopped = false;
            StopAllCoroutines();
            //ChangeAnimationState(ENEMY_IDLE);

            ChasePlayer();
            isIdling = false;

        }
        if ((distanceFromPlayer > sightRange && !PlayerHealth.isDead && !isIdling) || (isStunned))
        {
            if (!isStunned) //normal version
            {
                Debug.Log("Enemy should idle");
                thisEnemy.isStopped = true;
                StopAllCoroutines();
                IdleMode();
            }
            if (isStunned) //stunned version
            {
                //stunParticles.Play(); // play particles
                Debug.Log("Enemy should Stun");
                thisEnemy.isStopped = true;
                StopAllCoroutines();
                IdleMode();
            }
        }

        if ((distanceFromPlayer <= attackRange && !isAttacking && !PlayerHealth.isDead) && (!isStunned))
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

        //timer for cooldown
        if ((stunCooldown) && (cooldownRemaining > 0)) { cooldownRemaining -= Time.deltaTime; } //counts down
        if ((stunCooldown) && (cooldownRemaining <= 0)) { stunCooldown = false; } //ends at 0
        if (!stunCooldown) { cooldownRemaining = 10f; } //reset timer
        if(cheatOn) { stunCooldown = false; }
        //timer for how long stun lasts
        if ((isStunned) && (stunRemaining > 0))
        {
            stunRemaining -= Time.deltaTime;
        }
        if ((isStunned) && (stunRemaining <= 0))
        {
            isStunned = false;
        }
        if (!isStunned)
        {
            stunRemaining = 4.5f;
        }

    }

    private void IdleMode()
    {
        ChangeAnimationState(ENEMY_IDLE);
        isIdling = true;
    }

    private void ChasePlayer()
    {
        //stunParticles.Stop();
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

    //stun mechanic
    private void OnTriggerEnter(Collider other) // if enter collision
    {
        if(other.gameObject.CompareTag("Stun"))
        {
            //stun if tree
            if(isStunned) //if stun is already happening
            {
                //keep stun as is
                return;
            }
            if((!stunCooldown) && (!isStunned)) //if cooldown is done AND enemy isn't already stunned
            {
                //trigger stun
                stunParticles.Play();
                isStunned = true;
                stunCooldown = true;
            }
            if((stunCooldown) && (!isStunned)) //if cooldown is still going
            {
                //prevent stun
                isStunned = false;
            }
        }
    }

    //cooldown cheat
    public void cooldownCheat()
    {
        cheatOn = !cheatOn;
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
