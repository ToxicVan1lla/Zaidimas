using System.Collections;
using UnityEngine;

public class Tornado : EnemyMove
{
    [SerializeField] Animator anim;
    [SerializeField] private float duration;
    private bool isSpinning;
    private bool isTurning;
    private float defaultSpeed;
    private void Start()
    {
        defaultSpeed = speed;
    }
    private void Update()
    {

        if (isSpinning)
        {
            if (boxCollider.IsTouching(playerCollider))
            {
                Attack();
                stopCounter = 0.5f;
            }
            duration -= Time.deltaTime;
            Move();
            if (playerBody.position.x > transform.position.x && speed < 0 && !isTurning)
            {
                StartCoroutine(turnAround());
            }
            if (playerBody.position.x < transform.position.x && speed > 0 && !isTurning)
            {
                StartCoroutine(turnAround());
            }
        }
        else
            speed = 0;
        if (duration < 0)
        {
            isSpinning = false;
            speed = 0;
            anim.SetTrigger("Disappear");
        }
            
    }
    private void Disappear()
    {
        Destroy(gameObject);
    }
    private void startSpinning()
    {
        anim.SetTrigger("Spin");
        speed = defaultSpeed * ((playerBody.position.x > transform.position.x) ? 1 : -1);
        isSpinning = true;
    }
    private IEnumerator turnAround()
    {
        isTurning = true;
        yield return new WaitForSeconds(0.2f);
        speed *= -1;
        isTurning = false;
    }
   
}
