using UnityEngine;

public class SwordEnemy : EnemyMove
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
