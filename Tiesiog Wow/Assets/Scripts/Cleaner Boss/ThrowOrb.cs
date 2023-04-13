using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowOrb : MonoBehaviour
{
    public string objectName;
    public Vector2 landPosition;
    private bool fall = true;
    private Rigidbody2D body;

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (transform.position.x == landPosition.x && transform.position.y == landPosition.y)
            fall = false;
        if (fall)
        {
            transform.position = Vector2.MoveTowards(transform.position, landPosition, 7 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
            fall = false;
    }

}
