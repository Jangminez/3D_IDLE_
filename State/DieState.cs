using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
    private UnitBase unit;

    float time = 0f;

    public DieState(UnitBase unit)
    {
        this.unit = unit;
    }

    public void Enter()
    {
        time = 0f;
        unit.Agent.isStopped = true;
        unit.Animator.Play("Death");
    }

    public void Exit()
    {
        unit.IsDie = false;
    }

    public void Update()
    {
        time += Time.deltaTime;

        if (time >= 3f)
        {
            if (unit.TryGetComponent(out Player player))
            {
                player.Respawn();
                player.stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                unit.DestoryObject();
            }

            time = 0f;
        }
    }
}
