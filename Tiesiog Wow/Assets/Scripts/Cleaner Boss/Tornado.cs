using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : EnemyMove
{
    private bool tornado = false;
    private bool move = false;
    private int direction;
    private bool corutine = false;
    private float duration;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            tornado = true;
        if (tornado)
        {
            tornado = false;
            StartCoroutine(timer());
            move = true;
            direction = Random.Range(0, 2) * 2 - 1;
            speed *= direction;

        }
        if(move)
        {
            Move();
            if (!corutine)
            {
                duration = Random.Range(0.5f, 1);
                StartCoroutine(switchDirection());

            }
        }

    }
    private IEnumerator switchDirection()
    {
        Debug.Log("WW");
        corutine = true;
        yield return new WaitForSeconds(duration);
        speed *= -1;
        corutine = false;
    }
    private IEnumerator timer()
    {
        yield return new WaitForSeconds(5);
        enemyBody.velocity = Vector2.zero;
        move = false;
    }
}
