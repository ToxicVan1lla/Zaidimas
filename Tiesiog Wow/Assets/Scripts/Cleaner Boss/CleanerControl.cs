using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerControl : EnemyMove
{
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

    [SerializeField] private float attackRange;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask Player;
    [SerializeField] private BoxCollider2D BC;
    [SerializeField] private float attackCooldown;
    private float attackCounter;
    private bool isDoingMeleeAttack = false;

    private void Start()
    {
        defaultSpeed = speed;
    }
    void Update()
    {
        if (Mathf.Abs(playerBody.position.x - enemyBody.position.x) < 5)
            fightHasBegun = true;
        if(fightHasBegun)
        {
            if(justCaughtBroom)
            {
                StartCoroutine(runAwayCounter());
            }

            Move();
            if (boxCollider.IsTouching(playerCollider))
            {
                Attack();
            }
            if(!isRunning)
            {
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
                    anim.SetTrigger("Throw");
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

    }
    private void activateBroom()
    {
        GameObject GO =  Instantiate(broom, new Vector3(enemyBody.position.x + -1.2f * Mathf.Sign(enemyBody.transform.localScale.x), enemyBody.position.y, 0), transform.rotation);
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
    private bool playerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, Player);
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(BC.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
        new Vector3(BC.bounds.size.x * attackRange, BC.bounds.size.y, BC.bounds.size.z));
    }
}
