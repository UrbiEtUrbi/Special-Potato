using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phillip : Creature
{


    [SerializeField]
    Vector3 CurrentSpeed;

    [SerializeField]
    float MaxSpeed;


    public override void ToggleActive(bool _isActive)
    {
        base.ToggleActive(_isActive);

        _Animator.SetBool("IsFlying", true);
        _Animator.SetBool("IsSleeping", false);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (returnFromFixedUpdate)
        {
            return;
        }

        if (!IsActive)
        {
            return;
        }


        var direction = (ControllerGame.Player.transform.position - transform.position).normalized;


        CurrentSpeed += direction * Speed * Time.fixedDeltaTime;

        if (CurrentSpeed.magnitude > MaxSpeed)
        {
            CurrentSpeed = CurrentSpeed.normalized * MaxSpeed;
        }

        transform.position += CurrentSpeed;

        if (direction.x > 0 && this.direction == -1 || direction.x < 0 && this.direction == 1)
        {
            CurrentSpeed = default;
            turningAround = true;
            PauseTimer = PauseTime;
        }
    }

}
