using System.Collections.Generic;
using UnityEngine;

public class Miner : EnemyMove
{
    [SerializeField] Animator anim;
    [SerializeField] private float sightRange;
    [SerializeField] private float distanceWhenStopsAttacking;
    private float startX;
    private bool isAttacking;
    private float defaultSpeed;
    private bool isGoingBack;
    private float goingBackCounter;
    private float attackCooldown = 0.2f;
    private float attackCooldownCounter;
    private EnemyHealth enemyHealth;
    private float idleCounter = 0.2f;
    private float whichWayFacingWhenStarted;
    private void Start()
    {
        whichWayFacingWhenStarted = 1 * Mathf.Sign(transform.localScale.x);
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        startX = transform.position.x;
        defaultSpeed = speed;
        speed = 0;
    }

    private void Update()
    {
        if (!isAttacking && !isGoingBack && (1 * Mathf.Sign(transform.localScale.x) != whichWayFacingWhenStarted))
            Flip();
        if (enemyHealth.gotHit)
            anim.speed = 0;
        else
            anim.speed = 1;
        Move();
        attackCooldownCounter -= Time.deltaTime;
        idleCounter -= Time.deltaTime;
        if (boxCollider.IsTouching(playerCollider) && attackCooldownCounter <= 0)
        {
            Attack();
            anim.SetTrigger("Attack");
            attackCooldownCounter = attackCooldown;
        }
        if (!isAttacking && seesPlayer() && idleCounter <= 0 && playerInAttackRange())
        {
            anim.SetBool("Put axe away", true);

        }
        if(isGoingBack)
        {

            goingBackCounter += Time.deltaTime;
            if(playerInAttackRange())
            {
                isAttacking = true;
                isGoingBack = false;
                speed = defaultSpeed;
                goingBackCounter = 0;
            }
            if(Mathf.Abs(transform.position.x - startX) < 0.3f || goingBackCounter > 10)
            {
                anim.SetTrigger("Pull out axe");
                goingBackCounter = 0;
                isGoingBack = false;
                speed = 0;
            }
        }
        if(isAttacking)
        {
            if (!playerInAttackRange())
                isAttacking = false;

            if(!isAttacking)
            {
                if (startX > transform.position.x && transform.localScale.x < 0)
                {
                    Flip();
                }
                if (startX < transform.position.x && transform.localScale.x > 0)
                {
                    Flip();
                }
                isGoingBack = true;
                speed = 5;
            }

            if (playerBody.position.x > transform.position.x && transform.localScale.x < 0 && stopCounter <= 0)
            {

                stopCounter = 0.15f;
                enemyBody.velocity = Vector2.zero;
                Flip();
            }
            if (playerBody.position.x < transform.position.x && transform.localScale.x > 0 && stopCounter <= 0)
            {

                stopCounter = 0.15f;
                enemyBody.velocity = Vector2.zero;
                Flip();
            }

        }
    }

    private void startRunning()
    {
        anim.SetBool("Put axe away", false);
        anim.SetTrigger("Run");
        isAttacking = true;
        speed = defaultSpeed;
        isGoingBack = false;
    }

    private bool playerInAttackRange()
    {
        if (Mathf.Abs(playerBody.position.x - transform.position.x) > distanceWhenStopsAttacking)
        {
            return false;
        }
        if (Mathf.Abs(playerBody.position.y - transform.position.y) > 5)
        if (Mathf.Abs(playerBody.position.y - transform.position.y) > 5)
        {
            return false;
        }
        return true;
    }
    private bool seesPlayer()
    {
        RaycastHit2D[] back = Physics2D.RaycastAll(boxCollider.bounds.center, new Vector2(-1 * Mathf.Sign(enemyBody.transform.localScale.x), 0), sightRange);
        System.Array.Sort(back, new RaycastHit2DComparer(boxCollider.bounds.center));

        foreach (RaycastHit2D i in back)
        {
            if (i.collider.tag == "Player")
                return true;
            if (i.collider.tag == "Ground" || i.collider.tag == "Wall")
                return false;
        }
        RaycastHit2D[] front = Physics2D.RaycastAll(boxCollider.bounds.center, new Vector2(-1 * Mathf.Sign(enemyBody.transform.localScale.x), 0), 2);
        System.Array.Sort(back, new RaycastHit2DComparer(boxCollider.bounds.center));

        foreach (RaycastHit2D i in back)
        {
            if (i.collider.tag == "Player")
                return true;
            if (i.collider.tag == "Ground" || i.collider.tag == "Wall")
                return false;
        }
        return false;
    }
    private void returnToRun()
    {
        anim.SetTrigger("Run");
    }
    private void returnToIdle()
    {
        idleCounter = 0.2f;
        anim.SetTrigger("Idle");
    }
}







public class RaycastHit2DComparer : IComparer<RaycastHit2D>
{
    private readonly Vector2 centerPoint;

    public RaycastHit2DComparer(Vector2 centerPoint)
    {
        this.centerPoint = centerPoint;
    }

    public int Compare(RaycastHit2D hit1, RaycastHit2D hit2)
    {
        float distance1 = Vector2.Distance(centerPoint, hit1.point);
        float distance2 = Vector2.Distance(centerPoint, hit2.point);

        return distance1.CompareTo(distance2);
    }
}
