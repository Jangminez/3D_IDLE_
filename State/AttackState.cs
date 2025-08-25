using UnityEngine;

public class AttackState : IState
{
    private UnitBase unit;

    private float attackCooldown;
    private float lastAttackTime;

    public AttackState(UnitBase unit)
    {
        this.unit = unit;
    }

    public void Enter()
    {
        if (unit.TryGetComponent(out Player player))
            attackCooldown = player.TotalAttackSpeed;
        else
            attackCooldown = 1 / unit.Stat.attackSpeed;

        unit.Agent.isStopped = true;
        lastAttackTime = -attackCooldown;

        // 공격 애니메이션
        unit.Animator.SetFloat("AttackSpeed", unit.Stat.attackSpeed);
        unit.Animator.Play("Attack");
    }

    public void Exit()
    {
        unit.Agent.isStopped = false;
    }

    public void Update()
    {
        if (unit.Target == null || !unit.IsTargetInRange(unit.Stat.attackRange) || unit.IsTargetDie())
        {
            unit.stateMachine.ChangeState(unit.IdleState);
            return;
        }

        unit.LookAtTarget();

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
        }
    }
}
