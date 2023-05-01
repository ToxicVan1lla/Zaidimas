using System.Collections;
using UnityEngine;

public class Broom : EnemyMove
{
    public int direction;
    public float accelataration;
    public float maxSpeed;
    private Coroutine change;
    private bool hasTurned = false;
    private void Start()
    {
        hasTurned = false;
        blocked = false;
         change = StartCoroutine(changeDirection());
    }
    
    private void Update()
    {
        if(blocked)
        {
            blocked = false;
            if(!hasTurned)
            {
                StopCoroutine(change);
                direction *= -1;

            }
            speed = 0;
        }
        Move();
        speed += accelataration * direction * Time.deltaTime;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

    }
    
    private IEnumerator changeDirection()
    {
        yield return new WaitForSeconds(0.35f);
            direction *= -1;
        hasTurned = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            direction *= -1;
            enemyBody.velocity = Vector2.zero;
            speed = 0;
            collision.GetComponent<CleanerControl>().justCaughtBroom = true;
            Destroy(gameObject);
        }
        if (collision.tag == "Player")
        {
            Attack();
  
        }
    }
}
