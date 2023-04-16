using System.Collections;
using UnityEngine;

public class Rat : EnemyMove
{
    [SerializeField] Animator anim;
    private bool transformed = false;
    private float cantFlip;
    public float attackRange;
    public float attackRate;
    private float attackCounter;
    private float defaultSpeed;
    private Coroutine attackCoroutine;
    private EnemyHealth enemyHealth;
    private bool isAttacking;
    private void Start()
    {
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        defaultSpeed = speed;
    }
    private void Update()
    {
        
        if(transformed)
        {
            if(enemyHealth.gotHit && isAttacking)
            {
                isAttacking = false;
                StopCoroutine(attackCoroutine);
                anim.speed = 1;
                returnToRunning();
            }

            if (playerBody.position.x > transform.position.x && transform.localScale.x < 0 && cantFlip <= 0)
            {

                stopCounter = 0.1f;
                cantFlip = 0.5f;
                Flip();
            }
            if (playerBody.position.x < transform.position.x && transform.localScale.x > 0 && cantFlip <= 0)
            {

                stopCounter = 0.1f;
                cantFlip = 0.5f;
                Flip();
            }
            if (boxCollider.IsTouching(playerCollider))
            {
                Attack();
                attackCounter = attackRate;
                cantFlip = 0.5f;
            }
            if(playerInAttackRange() && attackCounter <=0)
            {
                cantFlip = 100;
                attackCoroutine = StartCoroutine(JumpAttack());
                attackCounter = attackRate;

            }
            Move();
            cantFlip -= Time.deltaTime;
            attackCounter -= Time.deltaTime;

        }
        else
        {
            if (playerBody.position.x > transform.position.x && transform.localScale.x < 0)
            {
                Flip();
            }
            if (playerBody.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    private void finishTransforming()
    {
        transformed = true;
        anim.SetTrigger("Run");
    }
    private void returnToRunning()
    {
        isAttacking = false;
        cantFlip = 0.5f;
        speed = defaultSpeed;
        anim.SetTrigger("Run");
    }
    
    private bool playerInAttackRange()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(boxCollider.bounds.center, new Vector2(1 * Mathf.Sign(enemyBody.transform.localScale.x), 0), attackRange);
        
        foreach(RaycastHit2D i in hits)
        {
            if (i.collider.CompareTag("Player"))
            {
                return true;
            }

        }
            return false;

    }
    private IEnumerator JumpAttack()
    {
        isAttacking = true;
        stopCounter = 0.5f;
        anim.speed = 0;
        yield return new WaitForSeconds(0.1f);
        anim.speed = 1;
        speed = 11;
        anim.SetTrigger("Attack");
    }


}
