using UnityEngine;

public class PatrolMovement : EnemyMove
{
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 8, true);
        boxCollider = gameObject.GetComponent<BoxCollider2D>();        
    }

    private void Update()
    {

        Move();
        if (boxCollider.IsTouching(playerCollider))
        {
            Attack();
        }
    }
}
