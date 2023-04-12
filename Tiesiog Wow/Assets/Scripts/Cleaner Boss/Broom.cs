using System.Collections;
using UnityEngine;

public class Broom : EnemyMove
{
    public int direction;
    public float accelataration;
    public float maxSpeed;
    private void Start()
    {
        StartCoroutine(changeDirection());
    }
    
    private void Update()
    {
        Move();
        speed += accelataration * direction * Time.deltaTime;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

    }
    
    private IEnumerator changeDirection()
    {
        yield return new WaitForSeconds(0.5f);
            direction *= -1;
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
