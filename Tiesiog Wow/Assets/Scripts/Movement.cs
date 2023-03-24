using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float slideSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private float cayotiTime = 0.2f;
    private float cayotiTimeCounter;
    private int jumpCounter;
    private bool isFacingRight;
    private float accelerationSpeed = 7;
    private float decelerationSpeed = 7;
    private float accelerationInAir = 10;
    private float decelerationInAir = 10;
    private bool isWallSliding;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private bool  lettingGoJump;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
   
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        anim = gameObject.GetComponent<Animator>();
        isFacingRight = true;
    }


    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
        if (Input.GetButtonUp("Jump"))
            lettingGoJump = true;
        
        if (horizontalInput != 0 && (horizontalInput > 0) != isFacingRight)
            Flip();

        anim.SetBool("Run", Mathf.Abs(body.velocity.x) > 0.5 && !onWall());

        if (isGrounded())
        {
            jumpCounter = 2;
            cayotiTimeCounter = cayotiTime;
        }
        else
            cayotiTimeCounter -= Time.deltaTime;

        if (!isWallSliding)
            body.gravityScale = 7;

        if (isWallSliding)
            wallJumpCounter = wallJumpTime;
        else
            wallJumpCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Run();
        wallSlide();
        if (isWallSliding)
            wallJump();
        else
            Jump();
    }

    private void Run()
    {
        Debug.Log(body.velocity.x);
        float targetSpeed = horizontalInput * speed;
        float accelRate;
        if(cayotiTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationSpeed : decelerationSpeed;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationSpeed * accelerationInAir : decelerationSpeed * decelerationInAir;

        float speedDif = targetSpeed - body.velocity.x;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 0.9f) * Mathf.Sign(speedDif);
        body.AddForce(movement * Vector2.right, ForceMode2D.Force);

        addFriction();

    }
    private void addFriction()
    {
        if(cayotiTime > 0 && horizontalInput == 0)
        {
            float amount = Mathf.Min(Mathf.Abs(body.velocity.x), 0.2f);

            amount *= Mathf.Sign(body.velocity.x);
            body.AddForce(-amount * Vector2.right, ForceMode2D.Impulse);

        }
    }

    private void wallSlide()
    {
        if (onWall() && !isGrounded())
        {
            if (horizontalInput != 0 && (Mathf.Sign(horizontalInput)==1) == isFacingRight)
                isWallSliding = true;
        }
        else
            isWallSliding = false;
        if (isWallSliding)
        {
            body.gravityScale = 0;
            body.velocity = new Vector2(body.velocity.x, -slideSpeed);
        }


    }
    private void wallJump()
    {
        if(jumpBufferCounter > 0 && wallJumpCounter > 0)
        {
            body.velocity = new Vector2(-Mathf.Sign(body.transform.localScale.x) * 10, 20);
            wallJumpCounter = 0;
            Flip();
            isWallSliding = false;
            jumpBufferCounter = 0;
            jumpCounter = 1;
        }

        lettingGoJump = false;
    }

    private void Jump()
    {
        if (jumpBufferCounter > 0 && (cayotiTimeCounter >= 0 || jumpCounter > 0))
        {
            jumpCounter--;
            if (cayotiTimeCounter < 0)
                jumpCounter = 0;

            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpBufferCounter = 0;

        }

        if (lettingGoJump && body.velocity.y > 0)
        {

            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
            cayotiTimeCounter = 0;
        }
            lettingGoJump = false;

    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
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

