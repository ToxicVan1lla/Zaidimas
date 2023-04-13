using System.Collections;
using UnityEngine;

public class CleanerControl : EnemyMove
{
    [SerializeField] GameObject door;
    public bool isThrowingBroom = false;
    [SerializeField] GameObject broom;
    [SerializeField] public Animator anim;
    [SerializeField] private float disntanceWhenThrowBroom;
    [SerializeField] private float maxTimeUntilThrow;
    private bool isCountingUntilThrow = false;
    private float timeUntilThrow;
    private bool hasToThrowBroom;
    private Coroutine rutine;
    private bool fightHasBegun = false;
    [HideInInspector] public float defaultSpeed;
    private bool isTurning = false;
    [HideInInspector] public bool justCaughtBroom = false;
    private bool isRunning;
    private GameObject GO;

    [SerializeField] private float attackRange;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask Player;
    [SerializeField] private LayerMask Wall;
    [SerializeField] private BoxCollider2D BC;
    [SerializeField] private float attackCooldown;
    private float attackCounter;
    private bool isDoingMeleeAttack = false;
    private EnemyHealth enemyHealth;
    private bool stage2 = false;
    private bool stage1 = true;
    [SerializeField] private float secondPhaseHealth;
    [SerializeField] private GameObject wings;
    public float leftWallX, rightWallX, groundY;
    private bool moveToCenter;
    private BoxCollider2D wingsCollider;
    private bool movingVertically;
    [SerializeField] GameObject Orb;
    private float middleRoomX;
    public GameObject leftArm, rightArm;
    private float coordinateX;
    private Vector2 throwPosition;

    private bool throwingOrb;
    private float timeUntilStop = 10;
    private float untilTurn = 0;
    private float untilLastTurnCounter = 0;
    [SerializeField] private float flyingSpeed;

    private void Start()
    {
        middleRoomX = (rightWallX + leftWallX) / 2;
        wingsCollider = wings.gameObject.GetComponent<BoxCollider2D>();
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        defaultSpeed = speed;
    }
    void Update()
    {
        if (Mathf.Abs(playerBody.position.x - enemyBody.position.x) < 5)
        {
            door.SetActive(true);
            fightHasBegun = true;
        }
        if (boxCollider.IsTouching(playerCollider) || wingsCollider.IsTouching(playerCollider))
        {
            Attack();
        }

        if(fightHasBegun)
        {
            if(stage1)
            {

                if(justCaughtBroom)
                {
                    StartCoroutine(runAwayCounter());
                }

                Move();
                if (enemyHealth.gotHit)
                    stopCounter = 0;
                if (timeUntilStop <= 0 && !isRunning && !isThrowingBroom)
                    stopCounter = 2;
                else
                    timeUntilStop -= Time.deltaTime;

                if (timeUntilStop <= 0 && !isRunning && !isThrowingBroom)
                    timeUntilStop = Random.Range(5, 15);
                
                if(!isRunning && stopCounter <=0)
                {
                    if (distanceToClosestWall() < 4)
                        StartCoroutine(runFromWall());
                    if(!isCountingUntilThrow && !hasToThrowBroom && !isThrowingBroom)
                    {
                        timeUntilThrow = Random.Range(1, maxTimeUntilThrow);
                        rutine = StartCoroutine(countToThrow());
                    }
                    if (enemyBody.position.x > playerBody.position.x && enemyBody.transform.localScale.x < 0 && !isThrowingBroom && !isTurning)
                    {
                        StartCoroutine(turnAround());
                    }
                    else if(enemyBody.position.x < playerBody.position.x && enemyBody.transform.localScale.x > 0 && !isThrowingBroom && !isTurning)
                    {
                        StartCoroutine(turnAround());
                    }
                    if (!isThrowingBroom && (Mathf.Abs(playerBody.position.x - enemyBody.position.x) > disntanceWhenThrowBroom || hasToThrowBroom) && !isDoingMeleeAttack && attackCounter > 0.2f)
                    {
                        speed = 0;
                        StopCoroutine(rutine);
                        isCountingUntilThrow = false;
                        hasToThrowBroom = false;
                        isThrowingBroom = true;
                        anim.SetTrigger("ThrowBroom");
                    }
                    if (playerInAttackRange() && !isThrowingBroom && attackCounter > attackCooldown && !isDoingMeleeAttack)
                    {
                        speed = 0;
                        isDoingMeleeAttack = true;
                        anim.SetTrigger("Hit");
                    }
                    attackCounter += Time.deltaTime;

                }

            }
            if(enemyHealth.health < 15 && stage1)
            {
                Destroy(GO);
                StopAllCoroutines();
                anim.SetTrigger("Transform");
                stage1 = false;
                enemyBody.gravityScale = 0;
                enemyBody.velocity = Vector2.zero;
                speed = flyingSpeed;
                boxCollider.enabled = false;
                wingsCollider.enabled = false;

            }
            if(stage2)
            {
                if(moveToCenter)
                {
                    if (enemyBody.position == new Vector2(middleRoomX, groundY + 6))
                    {
                    boxCollider.enabled = true;
                    wingsCollider.enabled = true;

                        moveToCenter = false;
                    }
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(middleRoomX, groundY + 6), 3 * Time.deltaTime);
                    return;
                }
                if (throwingOrb)
                {
                    enemyBody.velocity = new Vector2(0, enemyBody.velocity.y);
                    stopCounter = 1;
                }
                else
                    stopCounter = 0;
                
                Move();
                verticalMovement();
                if(!throwingOrb)
                    horizontalMovement();
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    throwingOrb = true;
                    coordinateX = Random.Range(leftWallX + 1, rightWallX - 1);
                    if (transform.position.x > coordinateX)
                    {
                        if (transform.localScale.x > 0)
                        {
                            throwPosition = leftArm.transform.position;
                            anim.SetTrigger("ThrowLeftOrb");

                        }
                        else
                        {
                            throwPosition = rightArm.transform.position;
                            anim.SetTrigger("ThrowRightOrb");
                        }
                    }
                    else
                    {
                        if (transform.localScale.x < 0)
                        {
                            throwPosition = leftArm.transform.position;
                            anim.SetTrigger("ThrowLeftOrb");

                        }
                        else
                        {
                            throwPosition = rightArm.transform.position;
                            anim.SetTrigger("ThrowRightOrb");
                        }
                    }
                    StartCoroutine(returnToIdle());
                }


            }

        }

    }

    private void horizontalMovement()
    {
        if ((Mathf.Abs(leftWallX - transform.position.x) < 4 || Mathf.Abs(rightWallX - transform.position.x) < 4) && untilLastTurnCounter <= 0)
        {
            untilLastTurnCounter = 4;
            Flip();
        }
        else
            untilLastTurnCounter -= Time.deltaTime;
        if (untilTurn <= 0 && untilLastTurnCounter <= 0)
        {
            Flip();
            untilTurn = Random.Range(3f, 7f);
        }
        else
            untilTurn -= Time.deltaTime;
    }

    private void verticalMovement()
    {
        if (!movingVertically)
            StartCoroutine(moveDownAndUp());
    }

    private void throwOrb()
    {
        GO = Instantiate(Orb, new Vector3(throwPosition.x, throwPosition.y, 0), transform.rotation);
        GO.GetComponent<ThrowOrb>().objectName = "Tornado";
        GO.GetComponent<ThrowOrb>().landPosition = new Vector2(coordinateX, groundY);
    }
    private IEnumerator returnToIdle()
    {
        yield return new WaitForSeconds(1.5f);
        throwingOrb = false;
        anim.SetTrigger("Fly");

    }

    private IEnumerator moveDownAndUp()
    {
        movingVertically = true;
        enemyBody.velocity = new Vector2(enemyBody.velocity.x, -0.5f);
        yield return new WaitForSeconds(0.5f);
        enemyBody.velocity = new Vector2(enemyBody.velocity.x, 0.5f);
        yield return new WaitForSeconds(0.5f);
        movingVertically = false;
    }

    private void activateBroom()
    {
        GO =  Instantiate(broom, new Vector3(enemyBody.position.x + -1.2f * Mathf.Sign(enemyBody.transform.localScale.x), enemyBody.position.y, 0), transform.rotation);
        GO.GetComponent<Broom>().direction = (int)(-1 * Mathf.Sign(enemyBody.transform.localScale.x));

    }

    private void doDamage()
    {
        if (playerInAttackRange())
            Attack();
    }
    private void stopMeleeAttack()
    {
        speed = defaultSpeed;
        anim.SetTrigger("Walking");
        isDoingMeleeAttack = false;
        attackCounter = 0;
    }

    private void startPhase2()
    {
        if (transform.localScale.x < 0)
            Flip();
        wings.SetActive(true);
        anim.SetTrigger("Fly");
        stage2 = true;
        moveToCenter = true;

    }
    
    private IEnumerator runFromWall()
    {
        yield return new WaitForSeconds(3);
        if(distanceToClosestWall() < 4)
        {
                if (isRunning || isDoingMeleeAttack || isThrowingBroom || isTurning)
                    yield break;
            
            if (transform.position.x < middleRoomX && transform.localScale.x > 0)
                Flip();
            if (transform.position.x > middleRoomX && transform.localScale.x < 0)
                Flip();
            isRunning = true;
            anim.speed = 2;
            speed = -6;
            while (Mathf.Abs(transform.position.x - middleRoomX) > 0.2)
            {
                yield return null;
            }
            speed = defaultSpeed;
            anim.speed = 1;
            isRunning = false;

        }
    }

    private IEnumerator countToThrow()
    {
        isCountingUntilThrow = true;
        yield return new WaitForSeconds(timeUntilThrow);
        isCountingUntilThrow = false;
        hasToThrowBroom = true;
    }
    private IEnumerator turnAround()
    {
        isTurning = true;
        stopCounter = 0.5f;
        yield return new WaitForSeconds(0.5f);
        Flip();
        isTurning = false;
    }
    private IEnumerator runAwayCounter()
    {
        isThrowingBroom = false;
        justCaughtBroom = false;
        isRunning = true;
        anim.speed = 2;
        anim.SetTrigger("Walking");
        Flip();
        speed = -6;
        yield return new WaitForSeconds(1.5f);
        speed = defaultSpeed;
        anim.speed = 1;
        isRunning = false;
    }

    private float distanceToClosestWall()
    {
        float distance = Mathf.Infinity;
        RaycastHit2D[] hits= Physics2D.RaycastAll(boxCollider.bounds.center, Vector2.left, 20);
        foreach(RaycastHit2D i in hits)
        {
            if (i.collider.gameObject.layer == 6)
                distance = Mathf.Min(distance, i.distance);
        }
        hits = Physics2D.RaycastAll(boxCollider.bounds.center, Vector2.right, 20);
        foreach (RaycastHit2D i in hits)
        {
            if (i.collider.gameObject.layer == 6)
                distance = Mathf.Min(distance, i.distance);
        }
        return distance;
    }
    private bool playerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y - 1, boxCollider.bounds.size.z),
        0, Vector2.left, 0, Player);
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(BC.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
        new Vector3(BC.bounds.size.x * attackRange, BC.bounds.size.y -1, BC.bounds.size.z));
    }

}
