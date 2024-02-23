using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Entity, IHealth
{

    [BeginGroup("Health")]
    [EndGroup]
    [SerializeField]
    int MaxHealth;


    int currentHealth;


    [SerializeField]
    protected float Speed;

    [SerializeField]
    protected float PauseTime;

    [SerializeField]
    protected float StunTime;

    protected int direction;

    protected bool isStunned = false;
    protected bool isFrozen = false;

    [SerializeField]
    protected Animator _Animator;

    [SerializeField]
    SpriteRenderer art;


    [SerializeField]
    Vector2 frozenSize;

    [SerializeField]
    Vector2 frozenOffset;

    protected float PauseTimer = 0;

    protected float StunTimer = 0;


    protected int GroundMask;


    protected bool turningAround = false;

    protected bool returnFromFixedUpdate;
    protected virtual void FixedUpdate()
    {
        returnFromFixedUpdate = false;
        _Animator.SetBool("Stunned", isStunned);
        _Animator.SetBool("Frozen", isFrozen);


        if (PauseTimer > 0 || StunTimer > 0 || isFrozen)
        {
            returnFromFixedUpdate = true;
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
        
    }

    protected virtual void Start()
    {
        GroundMask = LayerMask.GetMask("Ground");
        direction = -1;

        currentHealth = MaxHealth;
    }


    public  virtual void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, MaxHealth);
        if (amount < 0)
        {
            //spawn damage vfx
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        ControllerGame.ControllerAttack.OnEnemyDied();
        //spawn death animation
        Destroy(gameObject);
    }

    public virtual void ChangeHealth(int amount, AttackType type)
    {
        if (type == AttackType.PlayerSword)
        {
            isStunned = true;
            StunTimer = StunTime;
        }
        else if (type == AttackType.IceSpike || type == AttackType.IceMelee)
        {
            SoundManager.Instance.Play("crate_ice_block");
            isFrozen = true;
            StunTimer = StunTime;
            gameObject.layer = LayerMask.NameToLayer("Ground");

            OnFreeze();
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.mass = 10f;
            rb.drag = 5f;
            rb.gravityScale = 10f;
        }
    }

    protected virtual void OnFreeze()
    {
        var c = GetComponent<BoxCollider2D>();
        c.size = frozenSize;
        c.offset = frozenOffset;
    }

    private void Update()
    {
        PauseTimer -= Time.deltaTime;
        StunTimer -= Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, ActivationDistance);

    }


    public void SetInitialHealth(int amount) {
        currentHealth = amount;
    }

    
}
