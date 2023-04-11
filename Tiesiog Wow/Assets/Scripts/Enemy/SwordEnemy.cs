using System.Collections;
using UnityEngine;

public class SwordEnemy : EnemyMove
{
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float colliderDistance;
    [SerializeField] LayerMask Player;
    private float attackCounter;
    private bool isAttacking = false;
    [SerializeField] private Animator animator;

    private void Update()
    {
        Move();
        if (boxCollider.IsTouching(playerCollider))
        {
            Attack();
        }
        if (seesPlayer() && attackCounter < 0 && !isAttacking)
        {
            StartCoroutine(dashAttack());
        }
        attackCounter -= Time.deltaTime;
    }

    private bool seesPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size + transform.right * sightRange, 0, Vector2.left, 0, Player);
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size + transform.right * sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private IEnumerator dashAttack()
    {
        animator.SetBool("AfterAttack", false);
        isAttacking = true;
        stopCounter = 1;
        int direction = (playerBody.position.x > enemyBody.position.x) ? 1 : -1;
        if (Mathf.Sign(direction) != Mathf.Sign(enemyBody.transform.localScale.x))
            Flip();
        yield return new WaitForSeconds(0.5f);

        enemyBody.position = new Vector2(enemyBody.position.x + 1f * direction, enemyBody.position.y);

        for (int i = 1; i <= 3; i++)
        {
            animator.SetTrigger("Attack");

            do
            {
                yield return null;
            } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < i);

        }
        animator.SetBool("AfterAttack", true);

        attackCounter = attackCooldown;

        isAttacking = false;
    }
    private void hit()
    {
        if (playerInRange())
            Attack();
    }
    private bool playerInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, Player);
        return hit.collider != null;
    }


}
