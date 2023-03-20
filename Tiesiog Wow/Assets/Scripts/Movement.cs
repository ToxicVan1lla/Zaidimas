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
    private Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput, wallJumpCooldown;
    private float vytautoDydis = 4;
    private float moveAccelaration = 10;
    private float cayotiTime = 0.15f;
    private float cayotiTimeCounter;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0.1f)
            transform.localScale = new Vector3(vytautoDydis, vytautoDydis, 1);
        else if (horizontalInput < -0.1f)
            transform.localScale = new Vector3(-vytautoDydis, vytautoDydis, 1);
        anim.SetBool("Run", horizontalInput != 0 && !onWall());

        if (isGrounded())
            cayotiTimeCounter = cayotiTime;
        else
            cayotiTimeCounter -= Time.deltaTime;

        if(!onWall())
            body.gravityScale = 7;


        if (wallJumpCooldown > 0.2f)
        {

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if (onWall())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
                if (Input.GetKey(KeyCode.Space) && !isGrounded())
                {
                    body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 15, jumpForce);
                    transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * vytautoDydis, vytautoDydis, 1);
                    wallJumpCooldown = 0;
                }
                else if (horizontalInput == 0)
                    body.velocity = new Vector2(0, -slideSpeed);
                else
                    body.velocity = new Vector2(0, climbSpeed);
            }

            else if (Input.GetKey(KeyCode.Space) && cayotiTimeCounter>=0)
                body.velocity = new Vector2(body.velocity.x, jumpForce);
            if (Input.GetButtonUp("Jump") && body.velocity.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.3f);
                cayotiTimeCounter = 0;
            }


        }
        else
            wallJumpCooldown += Time.deltaTime;

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
