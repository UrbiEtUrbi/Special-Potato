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
    bool onGround = true;


    [SerializeField]
    Vector2 groundCheckSize;
    [SerializeField]
    Vector2 groundCheckPosition;

    [SerializeField]
    Animator Animator;

    int GroundLayerMask;

    void OnEnable()
    {
        GroundLayerMask = LayerMask.GetMask("Ground");
        ControllerInput.Instance.Horizontal.AddListener(OnHorizontal);
        ControllerInput.Instance.Vertical.AddListener(OnVertical);
        ControllerInput.Instance.Jump.AddListener(OnJump);
        m_Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void OnDisable()
    {
        ControllerInput.Instance.Horizontal.RemoveListener(OnHorizontal);
        ControllerInput.Instance.Vertical.RemoveListener(OnVertical);
        ControllerInput.Instance.Jump.RemoveListener(OnJump);
    }

    void FixedUpdate()
    {

        if (!ControllerGame.Instance.IsGamePlaying)
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
            rb.velocity += new Vector2(0, JumpVelocity);
        }

        m_Sprite.flipX = HorizontalVelocity < 0;


        bool falling = !onGround;

        Animator.SetBool("IsWalking", Mathf.Abs(HorizontalVelocity) > 10f);
        Animator.SetBool("Falling", falling && rb.velocity.y < -0.01);
        Animator.SetBool("Jumping", falling && rb.velocity.y > 0.01);

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

    void OnJump(bool jump)
    {
        
        jumped = jump && onGround;


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(rb.position+ groundCheckPosition, groundCheckSize);
    }
}
