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
    private bool isFacingRight;
    private float accelerationSpeed = 7;
    private float decelerationSpeed = 7;
    private float accelerationInAir = 10;
    private float decelerationInAir = 20;

    private bool isWallSliding;

    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private int jumpCounter;
    private float cayotiTime = 0.2f;
    private float cayotiTimeCounter;

    private bool  lettingGoJump;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private bool hasControl = true;
    private PlayerAttack playerAttack;

    private int numberOfDashes = 1;
    public bool dash = false;
    [SerializeField] private float dashSpeed;
    private float dashCounter = float.PositiveInfinity;
    [SerializeField] private float dashTime;
    [SerializeField] SpriteRenderer spr;
    void Start()
    {
        playerAttack = gameObject.GetComponent<PlayerAttack>();
        body = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        anim = gameObject.GetComponent<Animator>();
        isFacingRight = true;
    }


    void Update()
    {
        if (!Menu.gameIsPaused)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown("left shift") && !playerAttack.isAttacking)
                dash = true;
            dashCounter += Time.deltaTime;

            if (hasControl)
                if (horizontalInput != 0 && (horizontalInput > 0) != isFacingRight)
                {
                    Flip();
                }
            if (dashCounter <= dashTime || numberOfDashes == 0)
                dash = false;
            anim.SetBool("Dash", dash == true);
            anim.SetBool("Slide", isWallSliding && !isGrounded() && !playerAttack.isAttacking);

            if (Input.GetButtonDown("Jump"))
                jumpBufferCounter = jumpBufferTime;
            else
                jumpBufferCounter -= Time.deltaTime;
            if (Input.GetButtonUp("Jump"))
                lettingGoJump = true;

            if (body.velocity.y < -0.5f && !isWallSliding && !playerAttack.isAttacking)
                anim.SetTrigger("Falling");

            anim.SetBool("Run", Mathf.Abs(body.velocity.x) > 0.5 && !onWall());
            anim.SetBool("Grounded", isGrounded() && !playerAttack.isAttacking);

            if (isGrounded())
            {
                numberOfDashes = 1;
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
    }

    private void FixedUpdate()
    {
        if(dash && dashCounter > dashTime)
        {
            spr.color = Color.blue;
            hasControl = false;
            Dash();
        }

        if(hasControl)
        {
            Run();
            wallSlide();
            if (isWallSliding)
                wallJump();
            else
                Jump();

        }
    }

    private void Dash()
    {
        jumpBufferCounter -= Time.deltaTime;
        body.gravityScale = 0;
        body.velocity = new Vector2(dashSpeed * Mathf.Sign(body.transform.localScale.x), 0);

    }

    private void Run()
    {

        // Debug.Log(body.velocity.x);
        float targetSpeed = horizontalInput * speed;
        float accelRate;
        if(cayotiTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationSpeed : decelerationSpeed;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationSpeed * accelerationInAir : decelerationSpeed * decelerationInAir;

        float speedDif = targetSpeed - body.velocity.x;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 0.9f) * Mathf.Sign(speedDif);
        body.AddForce(movement * Vector2.right, ForceMode2D.Force);
        body.velocity = new Vector2(Mathf.Clamp(-speed, body.velocity.x, speed), body.velocity.y);

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
            if (horizontalInput != 0 && (Mathf.Sign(horizontalInput) == 1) == isFacingRight)
                isWallSliding = true;
        }
        else if (onWall() && isGrounded() && horizontalInput != 0)
            isWallSliding = true;
        else
            isWallSliding = false;
        if (isWallSliding)
        {
            numberOfDashes = 1;
            body.gravityScale = 0;
            body.velocity = new Vector2(body.velocity.x, -slideSpeed);
        }


    }
    private void wallJump()
    {
        if(jumpBufferCounter > 0 && wallJumpCounter > 0)
        {
            if(!playerAttack.isAttacking)
                anim.SetTrigger("Jump");
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
            if(!playerAttack.isAttacking)
                anim.SetTrigger("Jump");
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

    public void endDash()
    {
        if (onWall())
            isWallSliding = true;
        numberOfDashes = 0;
        spr.color = Color.white;
        dash = false;
        dashCounter = 0;
        anim.SetBool("Dash", false);
        hasControl = true;
       // body.velocity = new Vector2(0, body.velocity.y);
        body.gravityScale = 7;
    }
    private void transitionToIdle()
    {
        anim.SetTrigger("TransitionToIdle");
    }

}

