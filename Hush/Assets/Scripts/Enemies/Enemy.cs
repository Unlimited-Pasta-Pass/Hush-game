using System;
using Common.Enums;
using Enemies.Enums;
using Game;
using Game.Models;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour, IEnemy
    {
        #region Parameters

        [Header("Parameters")]
        [SerializeField] private float visionAngle = 70.0f;
        [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private LayerMask targetLayerMask;

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

        private GameObject _player;
        private EnemyState _state = EnemyState.Patrolling;
        private int _nextPatrolIndex;

        #endregion

        #region Public Variables

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public bool IsAttacking {
            get
            {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(EnemyAnimator.Layer.UpperBody);
                return stateInfo.IsName(EnemyAnimator.State.Attack);
            }
        }

        private float _hitPoints;
        public float HitPoints
        {
            get => _hitPoints;
            private set
            {
                _hitPoints = value;
                if (_hitPoints <= 0f)
                {
                    _hitPoints = 0f;
                    Die();
                }
            }
        }

        #endregion

        #region Events

        private void OnEnable()
        {
            _player = GameObject.FindWithTag(Tags.Player);
        }

        private void Start()
        {
            InitializeEnemy();
        }

        private void Update()
        {
            // Enemy lost track of player
            if (_state == EnemyState.Searching && agent.remainingDistance <= agent.stoppingDistance && !HasPlayerLineOfSight())
            {
                SetState(EnemyState.Patrolling);
            }

            // Do movement update logic
            MoveEnemy();
        }

        private void Reset()
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

        private void OnDrawGizmos()
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

        #endregion
        
        #region Computed Getters

        private Guid ID => GetComponent<GuidComponent>().GetGuid(); 
        
        #endregion
    
        #region Private Methods

        private void MoveEnemy()
        {
            switch (_state)
            {
                case EnemyState.Attacking:
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

                case EnemyState.Patrolling:
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

                case EnemyState.Searching:
                {
                    break;
                }

                case EnemyState.Dead:
                {
                    var stateInfo = animator.GetCurrentAnimatorStateInfo(EnemyAnimator.Layer.Base);
                    if (stateInfo.IsName(EnemyAnimator.State.Dead) && stateInfo.normalizedTime >= 0.99f)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
            }

            GameManager.Instance.MoveEnemy(ID, transform);
            
            // Update animator
            animator.SetFloat(EnemyAnimator.Speed, agent.velocity.magnitude);
        }

        private bool HasPlayerLineOfSight()
        {
            var position = transform.position;
            position.y = agent.height / 2.0f;
        
            var playerPosition = _player.transform.position;
            playerPosition.y = position.y;
        
            Vector3 towardsPlayer = playerPosition - position;

            var closeToPlayer = Vector3.Distance(playerPosition, position) <= detectionRadius;
            var seePlayer = Physics.Raycast(position, towardsPlayer.normalized, out var hit, visionCollider.radius, targetLayerMask) && hit.collider.CompareTag(Tags.Player);
        
            return closeToPlayer || seePlayer;
        }
    
        private void FacePlayer()
        {
            Vector3 lookPos = _player.transform.position - transform.position;
            lookPos.y = 0;
            
            if (lookPos == Vector3.zero)
                return;
        
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4.0f * Time.deltaTime);  
        }

        #endregion
    
        #region Public Methods
        
        public void InitializeEnemy()
        {
            HitPoints = GameManager.Instance.GetEnemyHitPoints(ID);
            
            // Make sure to add the enemy to the HP dictionary if it's not already there
            GameManager.Instance.UpdateEnemyHitPoints(ID , HitPoints);

            var enemyTransform = GameManager.Instance.GetEnemyTransform(ID);
            if (enemyTransform != null)
            {
                transform.position = enemyTransform.Position.Get();
                transform.rotation = enemyTransform.Rotation.Get();
            }

            if (GameManager.Instance.IsEnemyAttacking(ID))
            {
                SetState(EnemyState.Attacking);
            }
        }
        
        public void Die()
        {
            animator.SetBool(EnemyAnimator.Dead, true);
            SetState(EnemyState.Dead);
        
            Killed.Invoke();
        }

        public void TakeDamage(float damage)
        {
            // TODO: Add animation
            HitPoints -= damage;

            GameManager.Instance.UpdateEnemyHitPoints(ID , HitPoints);
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
            var playerDir = transform.InverseTransformPoint(other.gameObject.transform.position).normalized;
            if (_state != EnemyState.Dead && (_state != EnemyState.Patrolling || Vector3.Angle(playerDir, Vector3.forward) < visionAngle))
            {
                // If we have line of sight, keep following player
                SetState(HasPlayerLineOfSight() ? EnemyState.Attacking : EnemyState.Searching);
            }
        }

        private void SetState(EnemyState state)
        {
            _state = state;
            if (state == EnemyState.Attacking)
            {
                GameManager.Instance.EnemyStartedAttacking(ID);
            }
            else
            {
                GameManager.Instance.EnemyStoppedAttacking(ID);
            }
        }

        #endregion
    }
}
