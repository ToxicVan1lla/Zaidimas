using UnityEngine;
using System.Collections;


public class ChargingEnemy : EnemyMove
{
    [SerializeField] private LayerMask Player;
    [SerializeField] private float range;
    [SerializeField] private float ChargeTime;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rollDuration;
    private float untilCharge;
    private CircleCollider2D circleCollider;
    private Coroutine roll;
    private bool isRollingForwards;
    [SerializeField] private Animator animator;
    void Start()
    {
       // animator = animator.GetComponent<Animator>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreLayerCollision(8, 8, true);

    }

    private void Update()
    {
        Move();
        if (boxCollider.IsTouching(playerCollider) || circleCollider.IsTouching(playerCollider))
        {
            Attack();
            if(isRollingForwards)
            {
                StopCoroutine(roll);
                StartCoroutine(RollBack());
                isRollingForwards = false;
            }
        }
        untilCharge -= Time.deltaTime;
        if(untilCharge < 0 && seesPlayer())
        {
            animator.SetBool("Roll", true);
            roll = StartCoroutine(RollForwards());
        }

    }
    
    private IEnumerator RollForwards()
    {
        isRollingForwards = true;
        boxCollider.enabled = false;
        speed = rollSpeed;
        yield return new WaitForSeconds(rollDuration);
        isRollingForwards = false;
        StartCoroutine(RollBack());
    }
    private IEnumerator RollBack()
    {
        speed = -rollSpeed;
        yield return new WaitForSeconds(rollDuration);
        speed = 0;
        boxCollider.enabled = true;
        untilCharge = ChargeTime;
        animator.SetBool("Roll", false);
    }

    private bool seesPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, Player); 
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
