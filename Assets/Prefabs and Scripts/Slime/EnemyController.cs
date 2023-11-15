using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 1f;
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0f)
            {
                Die();
            }
        }
        get
        {
            return health;
        }
    }

    public float damage = 3f;
    public float attackRange = 0.5f;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    
    public GameObject player;
    public ContactFilter2D movementFilter;

    private Animator animator;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private bool isMoving = false;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRange(attackRange))
        {
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();

            bool moved = TryMove(direction);

            if (!moved) // slide x
            {
                moved = TryMove(new Vector2(direction.x, 0f));
            }
            
            if (!moved) // slide y
            {
                moved = TryMove(new Vector2(0f, direction.y));
            }
            
            animator.SetBool("isMoving", moved);
        }
    }

    private bool TryMove(Vector2 normalizedDirection) // Pass a normalized Vector2
    {
        int count = rb.Cast(normalizedDirection, movementFilter, castCollisions, collisionOffset);
        
        if (count == 0)
        {
            rb.MovePosition(rb.position + (moveSpeed * Time.fixedDeltaTime * normalizedDirection));
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= range;
    }

    public void Die()
    {
        animator.SetTrigger("death");
    }
    
    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}
