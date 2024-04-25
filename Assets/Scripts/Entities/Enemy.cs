using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Entities {
    [RequireComponent(typeof(Animator), typeof(EntityMovement))]
    public class Enemy : Damageable {
        [Header("Script References")]
        [SerializeField] private Animator animator;
        [SerializeField] private EntityMovement entityMovement;
        [SerializeField] private Hitbox hitbox;
    
        public bool canMove = true;
        [Tooltip("Lower number means more agile")]
        [SerializeField] private float agility = 5f;
        [SerializeField] private float attackRange = 1;
    
        private Vector3 direction;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Death1 = Animator.StringToHash("Death");

        private void Awake() {
            EntityManager.TestEnemies.Add(this);
            OnDying += Death;
        }
    
        private void OnDisable() {
            EntityManager.EnemyDeath(this);
        }
    
        private void Start() {
            agility = Random.Range(agility, 1.5f * agility);
            StartCoroutine(EnumeratorFunctions.ActionAfterTime(agility, () => {
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
            CheckIfFlipObject(position.playerPos);
            direction = (position.targetPos - transform.position).normalized;
            
            entityMovement.MoveInDirection(direction);
            animator.SetFloat(Speed, direction.magnitude);
        
            if (ComparePosition(position.targetPos, transform.position, 0.1f)) {
                Attack();
            }
        }
    
        private static bool ComparePosition(Vector3 position1, Vector3 position2, float distance) {
            return Vector3.Distance(position1, position2) < distance;
        }
    
        private void Attack() {
            animator.Play("Attack");
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
            canMove = false;
        }
    }
}