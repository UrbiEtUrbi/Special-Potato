using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth
{

    [SerializeField]
    Vector3 SwordAttackPosition;

    [SerializeField]
    Vector3 SwordAttackSize;

    [SerializeField]
    Vector3 SpikeAttackSize;

    PlayerMovement movement;


    void Awake() {
        movement = GetComponent<PlayerMovement>();
    }

    public void ChangeHealth(int amount)
    {
       
    }

    public void Die()
    {
    }

    public void SetInitialHealth(int amount)
    {
    }

    void Attack()
    {


        ControllerGame.ControllerAttack.Attack(
           transform,
           false,
           AttackType.IceSpike,
           transform.position + new Vector3(movement.Direction * SwordAttackPosition.x, SwordAttackPosition.y, SwordAttackPosition.z)
           , SwordAttackSize,1,
           movement.Direction
           );

        return;

        ControllerGame.ControllerAttack.Attack(
            transform,
            true,
            AttackType.PlayerSword,
            transform.position + new Vector3(movement.Direction * SwordAttackPosition.x, SwordAttackPosition.y, SwordAttackPosition.z)
            , SwordAttackSize,
            1,
            movement.Direction);
    }

    public void ChangeHealth(int amount, AttackType type)
    {
      
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + SwordAttackPosition, SwordAttackSize);
    }
}
