using UnityEngine;
using Utils;
using Utils.Entity;

[RequireComponent(typeof(Animator), typeof(EntityMovement))]
public class TestEnemy : Damageable {
    [Header("Script References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private Hitbox hitbox;
    
    private void Awake() {
        EnemyManager.TestEnemies.Add(this);
        OnDying += Death;
    }

    private void OnDisable() {
        EnemyManager.TestEnemies.Remove(this);
    }

    public void MoveToPosition(Vector3 position) {
        var direction = (position - transform.position).normalized;
        
        entityMovement.MoveInDirection(direction);
        
        Debug.Log("MoveTo");
    }
    
    public static bool ComparePosition(Vector3 position1, Vector3 position2, float distance) {
        return Vector3.Distance(position1, position2) < distance;
    }

    public void Attack() {
        hitbox.gameObject.SetActive(true);
        hitbox.damage = 2.5f;
        Debug.Log("Attack");
        StartCoroutine(EnumeratorFunctions.ActionAfterTime(1, () => hitbox.gameObject.SetActive(false)));
    }

    private void Death() {
        Destroy(gameObject);
    }
}