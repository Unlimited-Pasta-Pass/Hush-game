using System;
using Common;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IEnemy
{
    #region Parameters

    [Header("Parameters")]
    [SerializeField] private float visionAngle = 70.0f;
    [SerializeField] private int hitPoints = 100;

    [Header("Attack")]
    [SerializeField] private float minAttackRange = 0.5f;
    [SerializeField] private float maxAttackRange = 1.0f;
    
    [Header("Speed")]
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float patrolSpeed = 2.0f;
    
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolRoute;
    
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
        Attacking,
        Dead
    }

    private CharacterController _player;
    private State _state = State.Patrolling;
    private int _nextPatrolIndex;

    #endregion

    #region Public Variables

    public bool IsAttacking {
        get
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(EnemyAnimator.Layer.UpperBody);
            return stateInfo.IsName(EnemyAnimator.State.Attack);
        }
    }
    
    public int HitPoints
    {
        get => hitPoints; 
        set
        {
            hitPoints = value;
            if (hitPoints <= 0)
                Die();
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

    private void Start()
    {
        _player = GameObject.FindWithTag(Tags.Player).GetComponent<CharacterController>();
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
        Gizmos.DrawLine(position, transform.TransformPoint(first));
        Gizmos.DrawLine(position, transform.TransformPoint(second));
    }

    void Update()
    {
        // Enemy lost track of player
        if (_state == State.Searching && agent.remainingDistance <= agent.stoppingDistance && !HasPlayerLineOfSight())
        {
            SetState(State.Patrolling);
        }

        // Do movement update logic
        MoveEnemy();
    }

    #endregion
    
    #region Private Methods

    private void MoveEnemy()
    {
        switch (_state)
        {
            case State.Attacking:
            {
                // Run to player & attack them if in range
                agent.speed = runSpeed;
                Vector3 player = _player.transform.position;
                Vector3 toPlayerNormalized = (player - transform.position).normalized;
                agent.destination = player - minAttackRange * toPlayerNormalized;
                if (!IsAttacking && agent.remainingDistance <= maxAttackRange - minAttackRange)
                {
                    PerformAttack();
                }
                
                // Rotate to face player when arrived
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }
                break;
            }

            case State.Patrolling:
            {
                // Go to next position in patrol route if arrived
                agent.speed = patrolSpeed;
                if (agent.remainingDistance < 1.0f && patrolRoute.Length > 0)
                {
                    agent.destination = patrolRoute[_nextPatrolIndex].position;
                    _nextPatrolIndex = (_nextPatrolIndex + 1) % patrolRoute.Length;
                }
                break;
            }

            case State.Searching:
            {
                break;
            }

            case State.Dead:
            {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(EnemyAnimator.Layer.Base);
                if (stateInfo.IsName(EnemyAnimator.State.Dead) && stateInfo.normalizedTime >= 0.99f)
                {
                    Despawn();
                }
                break;
            }
        }

        // Update animator
        animator.SetFloat(EnemyAnimator.Speed, agent.velocity.magnitude);
    }

    private bool HasPlayerLineOfSight()
    {
        var position = transform.position;
        position.y = agent.height / 2.0f;
        var playerPosition = _player.transform.position;
        playerPosition.y = position.y;
        Vector3 toPlayer = playerPosition - position;
        return toPlayer.magnitude <= _player.height / 2 || Physics.Raycast(position, toPlayer.normalized, out var hit, visionCollider.radius) && hit.collider.CompareTag(Tags.Player);
    }
    
    private void FacePlayer()
    {
        Vector3 lookPos = _player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4.0f * Time.deltaTime);  
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    #endregion
    
    #region Public Methods
    
    public void Die()
    {
        SetState(State.Dead);
        animator.SetBool(EnemyAnimator.Dead, true);
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

    public void OnVisionTrigger(Collider other)
    {
        // Only trigger on player
        if (!other.CompareTag(Tags.Player))
            return;

        // Test player in enemy vision range
        var player = other.gameObject;
        var playerDir = transform.InverseTransformPoint(player.transform.position).normalized;
        if (_state != State.Dead && (_state != State.Patrolling || Vector3.Angle(playerDir, Vector3.forward) < visionAngle))
        {
            // If we have line of sight, keep following player
            if (HasPlayerLineOfSight())
            {
                SetState(State.Attacking);
            }
            // Otherwise, keep going to last seen location
            else
            {
                SetState(State.Searching);
            }
        }
    }

    private void SetState(State state)
    {
        _state = state;
        if (state == State.Attacking)
        {
            GameMaster.AddToEnemyList(this.GetInstanceID());
        }
        else
        {
            GameMaster.RemoveFromEnemyList(this.GetInstanceID());
        }
    }

    #endregion
}
