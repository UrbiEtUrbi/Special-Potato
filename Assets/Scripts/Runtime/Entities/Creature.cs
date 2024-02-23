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
     

    protected bool isStunned = false;
    protected bool isFrozen = false;

    [SerializeField]
    protected Animator _Animator;


    protected virtual void Start()
    {
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



    public void SetInitialHealth(int amount) {
        currentHealth = amount;
    }

    public virtual void ChangeHealth(int amount, AttackType type)
    {
        ChangeHealth(amount);
    }
}
