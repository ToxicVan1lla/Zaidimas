using UnityEngine;

public class PatrolMovement : EnemyMove
{
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 8, true);
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
