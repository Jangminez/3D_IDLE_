public class IdleState : IState
{
    private UnitBase unit;

    public IdleState(UnitBase unit)
    {
        this.unit = unit;
    }

    public void Enter()
    {
        unit.Agent.isStopped = true;

        // 애니메이션
        unit.Animator.Play("Idle");
    }

    public void Exit()
    {
        unit.Agent.isStopped = false;
    }

    public void Update()
    {
        if (unit.Target != null && unit.IsTargetInRange(unit.Stat.chaseRange) && !unit.IsTargetDie())
        {
            unit.stateMachine.ChangeState(unit.ChasingState);
            return;
        }
    }
}
