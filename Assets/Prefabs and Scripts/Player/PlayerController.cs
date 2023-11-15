using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordHitbox swordHitbox;

    private Vector2 inputVector;
    private Rigidbody2D rb; // Rigidbody2D is used to move the player and to detect collisions
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); // List of collisions that will be filled with the results of the cast
    private bool canMove = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("yInput", -1f); // Set the default animation to down
    }
    
    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        if (inputVector != Vector2.zero && canMove)
        {
            bool moved = TryMove(inputVector);

            if (!moved) // Try to slide horizontally
            {
                moved = TryMove(new Vector2(inputVector.x, 0f));
            }
            
            if (!moved) // Try to slide vertically
            {
                moved = TryMove(new Vector2(0f, inputVector.y));
            }
            
            animator.SetBool("isMoving", moved);
            animator.SetFloat("xInput", inputVector.x);
            animator.SetFloat("yInput", inputVector.y);
            
            // If the player is attacking, set the sword hitbox's direction to the direction the player is facing
            // Processing the x direction before the y direction prioritizes the y axis
            switch (inputVector.x)
            {
                case < 0f:
                    swordHitbox.attackDirection = SwordHitbox.AttackDirection.Left;
                    break;
                case > 0f:
                    swordHitbox.attackDirection = SwordHitbox.AttackDirection.Right;
                    break;
            }

            switch (inputVector.y)
            {
                case < 0f:
                    swordHitbox.attackDirection = SwordHitbox.AttackDirection.Down;
                    break;
                case > 0f:
                    swordHitbox.attackDirection = SwordHitbox.AttackDirection.Up;
                    break;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        
        
    }
    
    private bool TryMove(Vector2 direction)
    {
        // Raycast to see if there are any collisions in the direction the player is moving
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction of the cast
            movementFilter, // Filter that determines what layers the cast will collide with
            castCollisions, // List of collisions that will be filled with the results of the cast
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // Distance that the cast will travel (where the player is moving next)

        // If no collisions are detected in the cast, move the player
        if (count == 0)
        {
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMove(InputValue movementValue)
    {
        inputVector = movementValue.Get<Vector2>();
    }

    private void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }
    
    // Attack uses the AttackDirection set in FixedUpdate
    public void SwordAttack()
    {
        LockMovement();
        swordHitbox.Attack();
        // UnlockMovement is called in the animator
    }

    // Used to lock and unlock movement while the attack animation is playing
    public void LockMovement()
    {
        canMove = false;
    }
    
    public void UnlockMovement()
    {
        swordHitbox.StopAttack();
        canMove = true;
    }
}
