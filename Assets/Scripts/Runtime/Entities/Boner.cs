using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boner : Creature
{


    [SerializeField]
    float Speed;

    [SerializeField]
    float PauseTime;

    [SerializeField]
    float StunTime;

    int direction;

    [SerializeField]
    Vector3 GroundCheckPosition;

    [SerializeField]
    Vector3 GroundCheckSize;

    [SerializeField]
    Vector3 WallCheckPostion;

    [SerializeField]
    Vector3 WallCheckSize;

    [SerializeField]
    SpriteRenderer art;



    float PauseTimer = 0;

    float StunTimer = 0;


    int GroundMask;


    bool turningAround = false;


    protected override void Start()
    {
        GroundMask = LayerMask.GetMask("Ground");
        direction = -1;
        base.Start();
    }

    private void FixedUpdate()
    {

        _Animator.SetBool("Stunned", isStunned);
        _Animator.SetBool("Frozen", isFrozen);


        if (PauseTimer > 0 || StunTimer > 0 || isFrozen)
        {
            return;
        }
        if (turningAround)
        {
            _Animator.speed = 1;
            turningAround = false;
            direction *= -1;
            art.flipX = !art.flipX;
        }

        if (isStunned)
        {
            isStunned = false;
        }
       
        var pos2 = new Vector2(transform.position.x, transform.position.y);
        var groundHit = Physics2D.OverlapBox(pos2 + new Vector2(-direction * GroundCheckPosition.x, GroundCheckPosition.y), GroundCheckSize, 0, GroundMask);
        var wallHit = Physics2D.OverlapBox(pos2+  new Vector2(-direction * WallCheckPostion.x, WallCheckPostion.y), WallCheckSize, 0, GroundMask);


       
        _Animator.SetBool("Walking", true);



        if (!groundHit || wallHit)
        {
            _Animator.speed = 0;

            PauseTimer = PauseTime;
            turningAround = true;
        }
        transform.position += new Vector3(direction * Speed * Time.fixedDeltaTime, 0, 0);
    }

    public override void ChangeHealth(int amount, AttackType type)
    {
        if (type == AttackType.PlayerSword)
        {
            isStunned = true;
            StunTimer = StunTime;
        }
        else if (type == AttackType.IceSpike || type == AttackType.IceMelee)
        {
            isFrozen = true;
            StunTimer = StunTime;
            gameObject.layer = LayerMask.NameToLayer("Ground");

            var c = GetComponent<BoxCollider2D>();
            c.size = new Vector2(12, 8);
            c.offset = new Vector2(0, -2);
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.mass = 10f;
            rb.drag = 5f;
            rb.gravityScale = 10f;
        }
    }

    private void Update()
    {
        PauseTimer -= Time.deltaTime;
        StunTimer -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + GroundCheckPosition, GroundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + WallCheckPostion, WallCheckSize);
        
    }

}
