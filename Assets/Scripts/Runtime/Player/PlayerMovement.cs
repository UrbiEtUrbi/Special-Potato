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

    SpriteRenderer m_Sprite;


    bool Slowing;

    float HorizontalSpeed;
    float HorizontalVelocity;

    void OnEnable()
    {
        ControllerInput.Instance.Horizontal.AddListener(OnHorizontal);
        m_Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void OnDisable()
    {
        ControllerInput.Instance.Horizontal.RemoveListener(OnHorizontal);
    }

    void FixedUpdate()
    {

        if (!ControllerGame.Instance.IsGamePlaying)
        {
            rb.bodyType = RigidbodyType2D.Static;
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

        m_Sprite.flipX = HorizontalVelocity > 0;


    }


    void OnHorizontal(float amount) {

        HorizontalSpeed = amount*HorizontalAcc;

        Slowing = Mathf.Abs(amount) <0.1f;
    }
}
