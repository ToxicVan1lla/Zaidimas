using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : EnemyMove
{
    [HideInInspector] public Vector2 positionToLand;
    private bool landed = false;
    private void Update()
    {
        if(!landed)
            transform.position = Vector2.MoveTowards(transform.position, positionToLand, 3 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
