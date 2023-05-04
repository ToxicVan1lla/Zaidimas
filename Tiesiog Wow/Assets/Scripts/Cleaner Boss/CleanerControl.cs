using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerControl : EnemyMove, IDataPersistence
{
    public bool isThrowingBroom = false;
    [SerializeField] GameObject broom;
    [SerializeField] public Animator anim;
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
    private bool isCleaning;
    private bool hasToSpawnRat, hasToSpawnTornado;
    private float timeUntilNextRat = 5, timeUntilNextTornado = 10;
    [HideInInspector] public List<GameObject> GoHolder = new List<GameObject>();
    private string objectName;
    private bool noEnemies;
    public float maxTimeUntilRatSpawn, maxTimeUntilTornadoSpawn;
    public float timeUntilLand, timeToSpendLanded;
    private float timeUntilLandCounter;
    private bool land;
    private float timeLanded;
    int listSize;
    [SerializeField] Animator wingsAnim;
    private Animator doorAnim;
    [SerializeField] private GameObject door;
    private DataPersistanceManager manager;

    private bool alive = true;
    public void LoadData(GameData data)
    {
        alive = data.cleanerBossAlive;
        if (!alive)
            Destroy(gameObject);
    }

    public void SaveData(ref GameData data)
    {
        if (!alive)
            data.cleanerBossAlive = false;
    }
    private void Start()
    {

        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
        doorAnim = GameObject.Find("Door").GetComponent<Animator>();
        middleRoomX = (rightWallX + leftWallX) / 2;
        wingsCollider = wings.gameObject.GetComponent<BoxCollider2D>();
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        defaultSpeed = speed;
    }
    void Update()
    {
        if (Mathf.Abs(playerBody.position.x - enemyBody.position.x) < 5 && Mathf.Abs(playerBody.position.y - enemyBody.position.y) < 5 && !fightHasBegun)
        {
            door.SetActive(true);
            doorAnim.SetTrigger("Close");
            fightHasBegun = true;
            anim.SetBool("Walking", true);
        }
        if (boxCollider.IsTouching(playerCollider) || wingsCollider.IsTouching(playerCollider))
            Attack();

        if(fightHasBegun)
        {
            if(stage1)
            {
                if(justCaughtBroom)
                {
                    StartCoroutine(runAwayCounter());
                }
                if(stopCounter <= 0 && !isThrowingBroom && !isRunning && !isDoingMeleeAttack)
                {
                    isCleaning = false;
                    anim.SetBool("Walking", true);
                }
                Move();
                if (enemyHealth.gotHit)
                {
                    isCleaning = false;
                    stopCounter = 0;
                }
                if (timeUntilStop <= 0 && !isRunning && !isThrowingBroom && !isDoingMeleeAttack && anim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    isCleaning = true;
                    anim.SetBool("Walking", false);
                    anim.SetTrigger("Clean");
                    stopCounter = 2;
                    timeUntilStop = Random.Range(5f, 10f);
                }
                else
                    timeUntilStop -= Time.deltaTime;

                
                
                if(!isRunning && stopCounter <=0)
                {
                    if (distanceToClosestWall() < 3)
                        StartCoroutine(runFromWall());
                    if(!isCountingUntilThrow && !hasToThrowBroom && !isThrowingBroom)
                    {
                        timeUntilThrow = Random.Range(1, maxTimeUntilThrow);
                        rutine = StartCoroutine(countToThrow());
                    }
                    if (enemyBody.position.x > playerBody.position.x && enemyBody.transform.localScale.x < 0 && !isThrowingBroom && !isTurning && !isCleaning)
                    {
                        StartCoroutine(turnAround());
                    }
                    else if(enemyBody.position.x < playerBody.position.x && enemyBody.transform.localScale.x > 0 && !isThrowingBroom && !isTurning && !isCleaning)
                    {
                        StartCoroutine(turnAround());
                    }
                    if (!isThrowingBroom && hasToThrowBroom && !isDoingMeleeAttack && attackCounter > 0.2f && !isRunning && !isCleaning)
                    {
                        speed = 0;
                        anim.SetBool("Walking", false);
                        StopCoroutine(rutine);
                        isCountingUntilThrow = false;
                        hasToThrowBroom = false;
                        isThrowingBroom = true;
                        anim.SetTrigger("ThrowBroom");
                    }
                    if (playerInAttackRange() && !isThrowingBroom && attackCounter > attackCooldown && !isDoingMeleeAttack && !isRunning && !isCleaning)
                    {
                        speed = 0;
                        isDoingMeleeAttack = true;
                        anim.SetBool("Walking", false);
                        anim.SetTrigger("Hit");
                    }
                    attackCounter += Time.deltaTime;

                }

            }
            if(enemyHealth.health <= 15 && stage1 && !isThrowingBroom)
            {
                Destroy(GO);
                StopAllCoroutines();
                player.GetComponent<Movement>().playerAttack.noCooldown = false;
                player.GetComponent<Movement>().playerAttack.attackSpeed = 1;
                Time.timeScale = 1;
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
                    timeUntilLandCounter = timeUntilLand;
                    if (enemyBody.position == new Vector2(middleRoomX, groundY + 7))
                    {
                    boxCollider.enabled = true;
                    wingsCollider.enabled = true;

                        moveToCenter = false;
                    }
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(middleRoomX, groundY + 7), 3 * Time.deltaTime);
                    return;
                }
                if (throwingOrb)
                {
                    enemyBody.velocity = new Vector2(0, enemyBody.velocity.y);
                    stopCounter = 1;
                }
                else
                    stopCounter = 0;
                timeUntilLandCounter -= Time.deltaTime;
                if (timeUntilLandCounter <= 0 && !throwingOrb)
                {
                    land = true;
                    enemyBody.velocity = Vector2.zero;

                }
                listSize = 0;
                foreach (GameObject i in GoHolder)
                    if (i != null)
                        listSize++;
                if(land && listSize == 0)
                {
                    wingsCollider.enabled = false;
                    if (Mathf.Abs(enemyBody.position.y - (groundY + 1.4f)) < 0.1)
                    {
                        wingsAnim.speed = 0;
                        timeLanded -= Time.deltaTime;
                        
                    }
                    if(timeLanded <= 0)
                    {
                        wingsAnim.speed = 1;
                        timeUntilLandCounter = timeUntilLand;
                        land = false;
                        timeLanded = timeToSpendLanded;
                        moveToCenter = true;
                    }
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, groundY + 1.4f), 3 * Time.deltaTime);
                    if(enemyHealth.health <= 0)
                    {
                        wingsAnim.speed = 1;
                        timeUntilLandCounter = timeUntilLand;
                        land = false;
                        timeLanded = timeToSpendLanded;
                        moveToCenter = true;
                    }

                    return;
                }
                if(!land)
                {
                    Move();
                    timeUntilNextRat -= Time.deltaTime;
                    timeUntilNextTornado -= Time.deltaTime;
                    if (timeUntilNextRat <= 0 )
                        hasToSpawnRat = true;
                    if (timeUntilNextTornado <= 0)
                        hasToSpawnTornado = true;
                    verticalMovement();
                    if(listSize==0 && !noEnemies)
                    {
                        noEnemies = true;
                        timeUntilNextRat = 0.5f;
                    }

                    if (!throwingOrb)
                    {
                        horizontalMovement();
                        if(hasToSpawnRat)
                        {
                            noEnemies = false;
                            timeUntilNextRat = Random.Range(4f, maxTimeUntilRatSpawn);
                            hasToSpawnRat = false;
                            objectName = "Rat";
                            spawnOrb();
                        }
                        else if (hasToSpawnTornado)
                        {
                            noEnemies = false;
                            timeUntilNextTornado = Random.Range(7f, maxTimeUntilTornadoSpawn);
                            hasToSpawnTornado = false;
                            objectName = "Tornado";
                            spawnOrb();
                        }

                }

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
            untilTurn = Random.Range(2f, 5f);
        }
        else
            untilTurn -= Time.deltaTime;
    }

    private void verticalMovement()
    {
        if (!movingVertically)
            StartCoroutine(moveDownAndUp());
    }
    private void spawnOrb()
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

    private void throwOrb()
    {
        GO = Instantiate(Orb, new Vector3(throwPosition.x, throwPosition.y, 0), transform.rotation);
        GO.GetComponent<ThrowOrb>().objectName = objectName;
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
        anim.SetBool("Walking", true);
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
        yield return new WaitForSeconds(5);
        if(distanceToClosestWall() < 3)
        {
                if (isRunning || isDoingMeleeAttack || isThrowingBroom || isTurning || isCleaning)
                    yield break;
            
            if (transform.position.x < middleRoomX && transform.localScale.x > 0)
                Flip();
            if (transform.position.x > middleRoomX && transform.localScale.x < 0)
                Flip();
            isRunning = true;
            anim.SetBool("Walking", true);
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
        anim.SetBool("Walking", true);
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
            if (i.collider.gameObject.layer == 0)
                distance = Mathf.Min(distance, i.distance);
        }
        hits = Physics2D.RaycastAll(boxCollider.bounds.center, Vector2.right, 20);
        foreach (RaycastHit2D i in hits)
        {
            if (i.collider.gameObject.layer == 0)
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
    private void OnDestroy()
    {
        if(door != null && enemyHealth.health <= 0)
        {
            door.SetActive(false);
            doorAnim.SetTrigger("Open");
            alive = false;
            manager.save = true;
        }
    }

}
