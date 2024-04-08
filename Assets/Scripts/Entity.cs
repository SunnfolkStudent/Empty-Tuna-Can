using UnityEngine;
using Utils.Entity;

public class Entity : Damageable {
    public float movementSpeed = 1;
    public bool movementEnabled = true;
    
    public float staggerResistance;
    
    public Vector2 moveVector;
    
    protected Vector3 GravityVelocity;
    protected CharacterController CharacterController;

    protected void Awake() {
        CharacterController = GetComponent<CharacterController>();
    }
    
    private void FixedUpdate() {
        if (!movementEnabled) return;
        var move = new Vector3(moveVector.x, 0, moveVector.y) * (movementSpeed * 0.1f);
        move += GravityVelocity * Time.deltaTime;
        
        CharacterController.Move(move);
    }
}
