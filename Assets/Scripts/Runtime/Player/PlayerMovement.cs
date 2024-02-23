using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float MaxSpeed;

    [SerializeField]
    float HorizontalAcc;

    [SerializeField]
    float Drag;

    [SerializeField]
    float JumpVelocity;

    SpriteRenderer m_Sprite;


    bool Slowing;

    float HorizontalSpeed;
    float HorizontalVelocity;

    bool jumped;
    bool jumpApplied = true;
    [SerializeField]
    bool onGround = true;


    [SerializeField]
    Vector2 groundCheckSize;
    [SerializeField]
    Vector2 groundCheckPosition;

    [SerializeField]
    Animator Animator;

    int GroundLayerMask;

    public int Direction => m_Sprite.flipX ? -1 : 1;

    [SerializeField]
    float AttackCooldown;

    float AttackTimer = 0;

    void OnEnable()
    {
        jumpApplied = true;
        GroundLayerMask = LayerMask.GetMask("Ground");
        ControllerInput.Instance.Horizontal.AddListener(OnHorizontal);
        ControllerInput.Instance.Vertical.AddListener(OnVertical);
        ControllerInput.Instance.Jump.AddListener(OnJump);
        ControllerInput.Instance.Attack.AddListener(OnAttack);
        m_Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void OnDisable()
    {
        ControllerInput.Instance.Horizontal.RemoveListener(OnHorizontal);
        ControllerInput.Instance.Vertical.RemoveListener(OnVertical);
        ControllerInput.Instance.Jump.RemoveListener(OnJump);
        ControllerInput.Instance.Attack.RemoveListener(OnAttack);
    }

    private void Update()
    {
        AttackTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (jumpApplied && rb.velocity.y > 20 && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (!ControllerGame.Instance.IsGamePlaying)
        {
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                rb.velocity = new Vector2();
                Animator.SetBool("IsWalking", false);
                Animator.SetBool("Falling", false);
                Animator.SetBool("Jumping", !onGround);
                HorizontalVelocity = 0;
                rb.bodyType = RigidbodyType2D.Static;
            }
            return;
        }
        else if (!jumped && Mathf.Abs(HorizontalVelocity) < 0.1f && (Mathf.Abs(rb.velocity.y) < 2f && onGround) && Mathf.Abs(HorizontalSpeed) < 0.1f)
        {
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (Slowing)
        {
            HorizontalVelocity *= Drag;
        }
        else
        {

            HorizontalVelocity += HorizontalSpeed;
        }
        HorizontalVelocity = Mathf.Clamp(HorizontalVelocity, -MaxSpeed, MaxSpeed);

        rb.velocity = new Vector2(HorizontalVelocity, rb.velocity.y);
        if (jumped)
        {
            SoundManager.Instance.Play("jump");
            jumped = false;
            rb.velocity = new Vector2(rb.velocity.x, JumpVelocity);
        }

        if (!jumpApplied && !onGround)
        {
            jumpApplied = true;
        }

        if (HorizontalVelocity != 0)
        {
            m_Sprite.flipX = HorizontalVelocity < 0;
        }


        bool falling = !onGround;

        Animator.SetBool("IsWalking", Mathf.Abs(HorizontalVelocity) > 10f && !Animator.GetBool("IsAttacking"));
        if (rb.velocity.y == 0 && onGround)
        {
            Animator.SetBool("Falling", false);
            Animator.SetBool("Jumping", false);
        }
        else
        {
            Animator.SetBool("Falling", falling && rb.velocity.y < -10);
            Animator.SetBool("Jumping", falling && rb.velocity.y > -10);
        }

        onGround = Physics2D.OverlapBox(rb.position + groundCheckPosition, groundCheckSize, 0, GroundLayerMask) != null;
        if (falling && onGround)
        {
            SoundManager.Instance.Play("land");
        }

      
    }

   


    void OnHorizontal(float amount) {

        HorizontalSpeed = amount*HorizontalAcc;

        Slowing = Mathf.Abs(amount) <0.1f;
    }

    void OnVertical(float amount)
    {

        Animator.SetBool("IsSitting", amount < 0);
    }

    void OnAttack()
    {
        if (AttackTimer > 0)
        {
            return;
        }



       
        AttackTimer = AttackCooldown;

        if (false)
        {
            SoundManager.Instance.Play("melee_attack");
            Animator.SetBool("IsAttacking", true);
        }
        else
        {
            SoundManager.Instance.Play("melee_attack");
            Animator.SetBool("IsCasting", true);
        }
    }

    void OnEndAttack()
    {
        Animator.SetBool("IsAttacking", false);
        Animator.SetBool("IsCasting", false);
    }

    void OnJump(bool jump)
    {
        
        jumped = jump && onGround;
        if (jumped)
        {
            jumpApplied = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(rb.position+ groundCheckPosition, groundCheckSize);
    }
}
