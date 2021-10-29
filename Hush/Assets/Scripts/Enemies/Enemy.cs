using Common;
using UnityEngine;
using UnityEngine.AI;

// TODO: Hide an enemy with LOS plugin, it seems to not support hiding meshes on children game objects.
// We can hide the meshes individually, but it looks weird as each mesh uses its own extent to determine when to hide.
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IEnemy
{
    #region Parameters

    [Header("Parameters")]
    [SerializeField] private int hitPoints = 100;
    [SerializeField] private float visionAngle = 45.0f;
    [SerializeField] private float attackRange = 1.0f;
    
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private SphereCollider visionCollider;

    #endregion
    
    #region Private Variables
    
    private enum State
    {
        Patrolling,
        Searching,
        Attacking
    }

    private State _state = State.Patrolling;

    #endregion

    #region Public Variables

    // TODO: Not sure an ID is needed
    public int ID { get; set; }

    public bool IsAttacking {
        get
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(EnemyAnimator.Layer.UpperBody);
            return stateInfo.IsName(EnemyAnimator.State.Attack);
        }
    }

    #endregion

    #region Events

    void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!agent)
            agent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (!animator)
            animator = GetComponentInChildren<Animator>();
        visionCollider = GetComponent<SphereCollider>();
        if (!visionCollider)
            visionCollider = GetComponentInChildren<SphereCollider>();
    }

    void OnTriggerStay(Collider other)
    {
        // Only trigger on player
        if (!other.CompareTag(Tags.Player))
            return;

        // Test player in enemy vision range
        var player = other.gameObject;
        var playerDir = transform.InverseTransformPoint(player.transform.position).normalized;
        if (_state == State.Attacking || Vector3.Angle(playerDir, Vector3.forward) < visionAngle)
        {
            // If we have line of sight, keep following player
            var position = transform.position;
            position.y = agent.height / 2.0f;
            var playerPosition = player.transform.position;
            playerPosition.y = position.y;
            Vector3 toPlayer = playerPosition - position;
            if (Physics.Raycast(position, toPlayer.normalized, out var hit, visionCollider.radius) && hit.collider.CompareTag(Tags.Player))
            {
                _state = State.Attacking;
                agent.destination = player.transform.position;
            }
            
            // Otherwise, keep going to last seen location
            else
            {
                _state = State.Searching;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw some lines for vision cone
        Gizmos.color = Color.red;
        Vector3 first = 2.0f * new Vector3(
            Mathf.Cos((visionAngle + 90.0f) * Mathf.Deg2Rad), 0,
            Mathf.Sin((visionAngle + 90.0f) * Mathf.Deg2Rad));
        Vector3 second = 2.0f * new Vector3(
            Mathf.Cos((90.0f - visionAngle) * Mathf.Deg2Rad), 0,
            Mathf.Sin((90.0f - visionAngle) * Mathf.Deg2Rad));
        var position = transform.position;
        Gizmos.DrawLine(position, position + first);
        Gizmos.DrawLine(position, position + second);
    }

    void Update()
    {
        // Enemy reached destination 
        if (agent.remainingDistance <= agent.stoppingDistance)
            _state = State.Patrolling;
        
        // Do movement update logic
        MoveEnemy();
    }

    #endregion
    
    #region Private Methods

    private void MoveEnemy()
    {
        // Update animator
        animator.SetFloat(EnemyAnimator.Speed, agent.velocity.magnitude);
        if (!IsAttacking && _state == State.Attacking && agent.remainingDistance < attackRange)
        {
            PerformAttack();
        }
    }
    
    #endregion
    
    #region Public Methods
    
    public void Die()
    {
        // TODO: Add death animation
        Destroy(this);
    }

    public void TakeDamage(int amount)
    {
        // TODO: Add animation
        hitPoints -= amount;
        if (hitPoints <= 0)
            Die();
    }

    public void PerformAttack()
    {
        animator.SetTrigger(EnemyAnimator.BaseAttack);
    }

    #endregion
}
