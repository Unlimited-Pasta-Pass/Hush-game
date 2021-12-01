using System;
using System.Collections;
using Common.Enums;
using Enemies.Enums;
using Environment.Passage;
using Game;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : ObjectToggleLOS, IEnemy
    {

        #region Parameters

        [Header("Parameters")]
        [SerializeField] private float visionAngle = 70.0f;
        [SerializeField] private float detectionRadius = 3.0f;
        [SerializeField] private float soundPerceptionRadius = 4.0f;
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField] private AudioSource enemyHit;
        [SerializeField] private AudioSource deathSound;

        [Header("Attack")]
        [SerializeField] private float minAttackRange = 0.5f;
        [SerializeField] private float maxAttackRange = 1.0f;
        [SerializeField] private float attackRotationOffset = 0.0f;
        [SerializeField] private float backstabDamageModifier = 2.0f;

        [Header("Speed")]
        [SerializeField] private float runSpeed = 3.0f;
        [SerializeField] private float searchSpeed = 2.5f;
        [SerializeField] private float patrolSpeed = 1.0f;
        
        [Header("Type")]
        [SerializeField] private bool invisible = false;
        [Tooltip("The amount of time in seconds that the enemy gets revealed for when it takes damage. This only applies if `invisible` is set to true.")]
        [SerializeField] private float onHitRevealDuration = 1.0f;

        [Header("Patrol")]
        [SerializeField] private Transform[] patrolRoute;
    
        [Header("References")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private SphereCollider visionCollider;
        
        #endregion
    
        #region Private Variables

        private PlayerMovement _player;
        private EnemyState _state = EnemyState.Patrolling;
        private int _nextPatrolIndex;
        private bool _isStunned = false;

        private bool isPlayerInvisible => GameManager.Instance.GetIsPlayerInvisible();

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

        public float HitPoints => GameManager.Instance.GetEnemyHitPoints(ID);

        public override bool RevealOnEcholocate => invisible;

        #endregion

        #region Events

        private void Start()
        {
            var player = GameObject.FindWithTag(Tags.Player);
            if (player == null)
                throw new MissingComponentException("Missing Player in Scene");

            _player = player.GetComponent<PlayerMovement>();
            
            InitializeEnemy();
            if (invisible)
                Hide(true);
        }

        private void Update()
        {
            // Enemy lost track of player
            if (_state == EnemyState.Searching && agent.remainingDistance <= agent.stoppingDistance && !HasPlayerLineOfSight())
            {
                ResumePatrolFromClosestNode();
            }

            // Do movement update logic
            MoveEnemy();
        }

        public override void Reset()
        {
            base.Reset();
            
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
            // Notify game manager
            GameManager.Instance.MoveEnemy(ID, transform);
            
            // Update animator
            animator.SetFloat(EnemyAnimator.Speed, agent.velocity.magnitude);
            
            // Do not do state update if stunned
            if (_isStunned)
                return;
            
            switch (_state)
            {
                case EnemyState.Attacking:
                {
                    // Run to player & attack them if in range
                    agent.speed = runSpeed;
                    Vector3 player = _player.transform.position;
                    Vector3 toPlayerNormalized = (player - transform.position).normalized;
                    agent.destination = player - minAttackRange * toPlayerNormalized;
                    if (agent.remainingDistance <= maxAttackRange - minAttackRange)
                    {
                        // Rotate to face player when close
                        FacePlayer();
                        
                        // Attack if able to
                        if (!IsAttacking)
                            PerformAttack();
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
                    agent.speed = searchSpeed;
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
        
            return (closeToPlayer || seePlayer) && !isPlayerInvisible;
        }
    
        private void FacePlayer()
        {
            Vector3 lookPos = _player.transform.position - transform.position;
            lookPos.y = 0;
            
            if (lookPos == Vector3.zero)
                return;
        
            Quaternion targetRotation = Quaternion.Euler(Quaternion.LookRotation(lookPos).eulerAngles + new Vector3(0, attackRotationOffset, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4.0f * Time.deltaTime);  
        }

        private void ResumePatrolFromClosestNode()
        {
            float lowestDist = float.MaxValue;
            int closestNodeIndex = -1;
            for (int i = 0; i < patrolRoute.Length; i++)
            {
                float dist = Vector3.Distance(patrolRoute[i].position, transform.position);
                if (dist < lowestDist)
                {
                    closestNodeIndex = i;
                    lowestDist = dist;
                }
            }
            SetState(EnemyState.Patrolling);
            _nextPatrolIndex = closestNodeIndex > 0 ? closestNodeIndex : 0;
        }

        #endregion
    
        #region Public Methods
        
        public void InitializeEnemy()
        {
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
            // Disable colliders on NPC death to allow shooting spells through them
            foreach (var col in GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            
            deathSound.Play();
            animator.SetBool(EnemyAnimator.Dead, true);
            SetState(EnemyState.Dead);

            Killed.Invoke();
        }

        public void TakeDamage(float damage)
        {
            // Reveal if invisible
            if (invisible)
            {
                StartCoroutine(Reveal());
            }

            // Stun
            Stun(0.5f);
            
            // Take damage
            if (!GameManager.Instance.AttackEnemy(ID, _state == EnemyState.Patrolling ? backstabDamageModifier * damage : damage))
                Die();
            else
                SetState(EnemyState.Attacking);
        }

        public void PerformAttack()
        {
            enemyHit.Play();
            animator.SetTrigger(EnemyAnimator.BaseAttack);
        }

        public void OnVisionTrigger(Collider other)
        {
            // Only trigger on player
            // Only update state if not dead
            if (_state == EnemyState.Dead || !other.CompareTag(Tags.Player))
                return;

            // Compute distance and direction to player
            var playerPosition = _player.transform.position;
            var playerDir = transform.InverseTransformPoint(playerPosition).normalized;
            var playerDist = Vector3.Distance(transform.position, playerPosition);

            // Test player is in enemy vision range
            var isPatrolling = _state == EnemyState.Patrolling;
            var playerInVisionCone = Vector3.Angle(playerDir, Vector3.forward) < visionAngle;
            if (!isPatrolling || playerInVisionCone)
            {
                // If we have line of sight, keep following player
                SetState(HasPlayerLineOfSight() ? EnemyState.Attacking : EnemyState.Searching);
            }
            
            // Test player is making sound within sound perception radius
            var playerMadeSound = playerDist < soundPerceptionRadius && _player.IsRunning;
            if (playerMadeSound)
            {
                if (HasPlayerLineOfSight())
                {
                    SetState(EnemyState.Attacking);
                }
                else
                {
                    SetState(EnemyState.Searching);
                    agent.destination = playerPosition;
                }
            }
        }

        public void Stun(float duration)
        {
            // Only trigger if not already stunned
            if (!_isStunned)
            {
                // Play hit animation
                animator.SetTrigger(EnemyAnimator.TakeHit);
                _isStunned = true;
                
                Invoke(nameof(DisableStun), duration);
            }
        }

        private void DisableStun()
        {
            _isStunned = false;
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
        
        private IEnumerator Reveal()
        {
            Show();
            yield return new WaitForSeconds(onHitRevealDuration);
            Hide();
        }

        #endregion
    }
}
