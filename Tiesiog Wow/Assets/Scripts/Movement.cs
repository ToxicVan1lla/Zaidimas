using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput, verticalInput, wallJumpCooldown;
    private bool isClimbing, isLadder;
    private float vytautoDydis = 0.25f;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.2f)
            transform.localScale = new Vector3(vytautoDydis, vytautoDydis, 1);
        else if (horizontalInput < -0.1f)
            transform.localScale = new Vector3(-vytautoDydis, vytautoDydis, 1);

        if (wallJumpCooldown > 0.2f)
        {

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if(onWall())
            {
                body.gravityScale = 0;
                if (Input.GetKey(KeyCode.Space) && !isGrounded())
                {
                    body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 15, jumpForce);
                    transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * vytautoDydis, vytautoDydis, 1);
                    wallJumpCooldown = 0;
                }
                else if (horizontalInput == 0)
                    body.velocity = new Vector2(0, -slideSpeed);
            }

            else if (Input.GetKey(KeyCode.Space) && isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpForce);

            verticalInput = Input.GetAxis("Vertical");

        }
        else
            wallJumpCooldown += Time.deltaTime;

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
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

}
