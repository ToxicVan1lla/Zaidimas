using UnityEngine;

public class PatrolMovement : EnemyMove
{
    [SerializeField] Animator anim;
    [SerializeField] private float timeInOneSpot;
    [SerializeField] private float maxTimeUntilStop;
    private float timeUntilStop;
    private bool isCamping = false;

    private void Start()
    {
        timeUntilStop = Random.Range(2f, maxTimeUntilStop);
    }
    private void Update()
    {
        Move();
        if (boxCollider.IsTouching(playerCollider))
        {
            Attack();
        }
        if(isCamping && stopCounter <= 0)
        {
            isCamping = false;
            anim.SetTrigger("Go up");
            timeUntilStop = Random.Range(2f, maxTimeUntilStop);
        }

        if(!isCamping && timeUntilStop <= 0)
        {
            stopCounter = timeInOneSpot;
            isCamping = true;
            anim.SetTrigger("Go down");
        }
        timeUntilStop -= Time.deltaTime;
    }

    private void isDown()
    {
        anim.SetTrigger("Idle");
    }
    private void isUp()
    {
        anim.SetTrigger("Walk");
    }
}
