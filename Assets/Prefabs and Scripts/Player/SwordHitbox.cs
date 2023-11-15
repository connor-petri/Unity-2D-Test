using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public enum AttackDirection
    { Up, Down, Left, Right }
    
    public AttackDirection attackDirection;
    public float damage = 3f;
    
    // These are the offsets and sizes of the hitboxs for each direction
    public Vector2 upOffset;
    public Vector2 upSize;
    public Vector2 downOffset;
    public Vector2 downSize;
    public Vector2 leftOffset;
    public Vector2 leftSize;
    public Vector2 rightOffset;
    public Vector2 rightSize;
    
    private BoxCollider2D hitbox;

    private void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
    }
    
    private void AttackUp()
    {
        // Set the hitbox's offset and size to the upOffset and upSize
        hitbox.offset = upOffset;
        hitbox.size = upSize;
        hitbox.enabled = true;
    }
    
    private void AttackDown()
    {
        hitbox.offset = downOffset;
        hitbox.size = downSize;
        hitbox.enabled = true;
    }
    
    private void AttackLeft()
    {
        hitbox.offset = leftOffset;
        hitbox.size = leftSize;
        hitbox.enabled = true;
    }

    private void AttackRight()
    {
        hitbox.offset = rightOffset;
        hitbox.size = rightSize;
        hitbox.enabled = true;
    }

    public void Attack()
    {
        switch (attackDirection)
        {
            case AttackDirection.Up:
                AttackUp();
                break;
            case AttackDirection.Down:
                AttackDown();
                break;
            case AttackDirection.Left:
                AttackLeft();
                break;
            case AttackDirection.Right:
                AttackRight();
                break;
        }
    }

    public void StopAttack()
    {
        hitbox.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.Health -= damage;
        }
    }
}
