using Audio;
using FMODUnity;
using StateMachineBehaviourScripts;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Entities {
    [RequireComponent(typeof(Animator), typeof(EntityMovement))]
    public class Enemy : Damageable {
        [Header("Script References")]
        [SerializeField] private Animator animator;
        [SerializeField] private EntityMovement entityMovement;
        
        [Tooltip("Lower number means more agile")]
        [SerializeField] private float agility = 5f;
        [SerializeField] private float attackRange = 1;
        [SerializeField] private string[] attacks = {"Attack"};
        
        private Vector3 direction;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Death1 = Animator.StringToHash("Death");

        [field: Header("Sound Clips")]
        [field: SerializeField] private EventReference EnemyGetHit;

        private void Awake() {
            EntityManager.TestEnemies.Add(this);
            OnDying += Death;
            
            foreach (var stateBehaviour in animator.GetBehaviours<Attack>()) {
                stateBehaviour.damageable = this;
            }
        }
    
        private void OnDisable() {
            EntityManager.EnemyDeath(this);
        }
    
        private void Start() {
            agility = Random.Range(agility, 1.5f * agility);
            StartCoroutine(EnumeratorFunctions.ActionAfterTime(Random.Range(0.5f * agility, 3 * agility), () => {
                StartCoroutine(EnumeratorFunctions.ActionAtInterval(agility, () => {
                    canMove = !canMove;
                }));
            }));
        }
    
        private void Update() {
            animator.SetFloat(Speed, 0);
            if (!canMove) return;
            MoveToPosition();
        }
    
        private void MoveToPosition() {
            var position = EntityManager.GetTargetPosition(transform.position, attackRange);
            if (position == default) return;
            
            CheckIfFlipObject(position.playerRelativePos);
            
            Debug.Log(position.playerRelativePos, gameObject);
            
            if (position.playerRelativePos.With(y: 0, z: 0).magnitude < attackRange + 0.4f && position.playerRelativePos.With(x: 0, z: 0).magnitude < 0.1f) {
                Debug.Log("Attack", gameObject);
                Attack();
                return;
            }
            
            direction = (position.targetRelativePos - transform.position).normalized;
            
            entityMovement.MoveInDirection(direction);
            animator.SetFloat(Speed, direction.magnitude);
        }
        
        private void Attack() {
            animator.Play(attacks.GetRandom());
            canMove = false;
        }
    
        private void Death() {
            animator.SetTrigger(Death1);
            StopAllCoroutines();
            canMove = false;
        }
    
        private void DestroyObject() {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
    
        private void CheckIfFlipObject(Vector3 targetRelativeLocation) {
            if (targetRelativeLocation.x < 0) transform.localScale = transform.localScale.With(x: -1);
            else if (targetRelativeLocation.x > 0) transform.localScale = transform.localScale.With(x: 1);
        }
    
        protected override void Stagger() {
            if (animator.GetCurrentAnimationClip().name == "Death") return;
            animator.Play("Hit");
            AudioManager.Instance.PlayOneShot(EnemyGetHit, this.transform.position);
            canMove = false;
        }
    }
}