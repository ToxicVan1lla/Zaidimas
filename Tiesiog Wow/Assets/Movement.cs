using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeReference]private float speed;
    [SerializeReference] private float jumpForce;
    [SerializeField] private float climbSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private float verticalInput;
    private bool isClimbing;
    private bool isLadder;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.2f)
            transform.localScale = new Vector3(0.3f, 0.3f, 1);
        else if (horizontalInput < -0.1f)
            transform.localScale = new Vector3(-0.3f, 0.3f, 1);
        
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
 

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        verticalInput = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(verticalInput) > 0)
            isClimbing = true;

    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            body.gravityScale = 0;
            body.velocity = new Vector2(body.velocity.x, verticalInput * climbSpeed);
        }
        else
            body.gravityScale = 7;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            isLadder = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    
}
