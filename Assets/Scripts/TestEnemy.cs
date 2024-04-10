using UnityEngine;
using Utils.Entity;

public class TestEnemy : Damageable {
    [Header("Script References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private EnemyAwareness enemyAwareness;
}