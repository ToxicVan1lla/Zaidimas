using UnityEngine;

public class ChargingEnemy : EnemyMove
{
    [SerializeField] private LayerMask Player;
    [SerializeField] private float range;
    [SerializeField] private float ChargeTime;
    private float ChargeCounter;
    private bool isRunnig = false;
    private int direction;
    private float untilCharge;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 8, true);
    }

    private void Update()
    {

        Move();
        if (boxCollider.IsTouching(playerCollider))
        {
            Attack();
        }
        untilCharge -= Time.deltaTime;
        if(untilCharge < 0 && !isRunnig && seesPlayer())
        {
            direction = (playerBody.position.x > body.position.x) ? 1 : -1;
            if (direction != Mathf.Sign(body.transform.localScale.x))
                Flip();
            ChargeCounter = ChargeTime;
            isRunnig = true;
        }
        if (ChargeCounter > 0)
        {            
                ChargeCounter -= Time.deltaTime;
                speed = 5;
        }
        else if(isRunnig)
        {
            untilCharge = 1f;
            stopCounter = 0.2f;
            speed = 2;
            isRunnig = false;
        }

    } 

    private bool seesPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size + transform.right * range, 0, Vector2.left, 0, Player); 
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size + transform.right * range);
    }
}
