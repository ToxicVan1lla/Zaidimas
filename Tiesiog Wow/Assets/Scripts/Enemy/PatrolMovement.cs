using UnityEngine;

public class PatrolMovement : EnemyMove
{

    private void Update()
    {
        Move();
        if (boxCollider.IsTouching(playerCollider))
        {
            Attack();
        }
    }
}
