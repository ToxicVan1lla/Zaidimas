using System.Collections.Generic;
using UnityEngine;

public class Miner : EnemyMove
{
    [SerializeField] Animator anim;
    [SerializeField] private float sightRange;
    [SerializeField] private float distanceWhenStopsAttacking;
    private float startX;
    private float startY;
    private bool isAttacking;
    private float defaultSpeed;
    private bool isGoingBack;
    private float goingBackCounter;
    private float attackCooldown = 0.2f;
    private float attackCooldownCounter;
    private EnemyHealth enemyHealth;
    private float idleCounter = 0.2f;
    private float whichWayFacingWhenStarted;
    private float playerAboveCounter;
    private void Start()
    {
        whichWayFacingWhenStarted = 1 * Mathf.Sign(transform.localScale.x);
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        startX = transform.position.x;
        startY = transform.position.y;
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
            if(Mathf.Abs(transform.position.x - startX) < 0.5f || goingBackCounter > 10)
            {
                anim.SetTrigger("Pull out axe");
                goingBackCounter = 0;
                playerAboveCounter = 0;
                isGoingBack = false;
                speed = 0;
            }
        }
            if (Mathf.Abs(playerBody.position.y - transform.position.y) > 0.3f)
                playerAboveCounter += Time.deltaTime;
            else
                playerAboveCounter = 0;
        if(isAttacking)
        {
            if (!playerInAttackRange())
                isAttacking = false;

            if(!isAttacking)
            {
                if (Mathf.Abs(startY - transform.position.y) > 0.1)
                {
                    startX = findClosestWall();
                    startY = transform.position.y;
                    float whichWayShouldFace = 1;

                    if ((transform.localScale.x > 0 && startX < transform.position.x) || (transform.localScale.x < 0 && startX > transform.position.x))
                    {
                        whichWayShouldFace = -1;
                    }
                    if (Mathf.Sign(transform.localScale.x) * whichWayShouldFace != whichWayFacingWhenStarted)
                        whichWayFacingWhenStarted *= -1;
                }
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
                return;
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

    private float findClosestWall()
    {
        float closestDistance = Mathf.Infinity;
        float closestPosition = 0;
        RaycastHit2D[] back = Physics2D.RaycastAll(new Vector2(transform.position.x, boxCollider.bounds.center.y), new Vector2(-1 * Mathf.Sign(enemyBody.transform.localScale.x), 0), 100);

        foreach(RaycastHit2D i in back)
        {
            if((i.collider.tag == "Wall" || i.collider.tag == "Ground") && i.distance < closestDistance)
            {
                closestDistance = i.distance;

                if (transform.localScale.x < 0)
                    closestPosition = transform.position.x + i.distance;
                else
                    closestPosition = transform.position.x - i.distance;            
            }
        }

        RaycastHit2D[] front = Physics2D.RaycastAll(new Vector2(transform.position.x, boxCollider.bounds.center.y) , new Vector2(1 * Mathf.Sign(enemyBody.transform.localScale.x), 0), 100);

        foreach (RaycastHit2D i in front)
        {
            if ((i.collider.tag == "Wall" || i.collider.tag == "Ground") && i.distance < closestDistance)
            {
                closestDistance = i.distance;

                if (transform.localScale.x < 0)
                    closestPosition = transform.position.x - i.distance;
                else
                    closestPosition = transform.position.x + i.distance;
            }
        }
        return (closestDistance != Mathf.Infinity ? closestPosition : Mathf.Infinity);

    }
    private bool playerInAttackRange()
    {
        if (Mathf.Abs(playerBody.position.x - transform.position.x) > distanceWhenStopsAttacking)
        {
            return false;
        }
        
        if (playerAboveCounter > 1.5f)
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
        RaycastHit2D[] front = Physics2D.RaycastAll(boxCollider.bounds.center, new Vector2(1 * Mathf.Sign(enemyBody.transform.localScale.x), 0), 2);
        System.Array.Sort(front, new RaycastHit2DComparer(boxCollider.bounds.center));

        foreach (RaycastHit2D i in front)
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
