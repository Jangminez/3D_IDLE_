using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitBase : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }

    public NavMeshAgent Agent { get; protected set; }
    public Animator Animator { get; protected set; }

    public Transform Target { get; protected set; }

    [Header("Stat")]
    protected UnitStatSO statSO;
    [SerializeField] protected UnitStat unitStat;
    public UnitStat Stat { get => unitStat; }
    protected float currentHealth;

    public IdleState IdleState { get; private set; }
    public ChasingState ChasingState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DieState DieState { get; private set; }
    public bool IsDie { get; set; }

    private HpBar hpBar;
    public Action<float, float> onHealthChanged;

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        hpBar = GetComponent<HpBar>();

        stateMachine = new StateMachine();
        IdleState = new IdleState(this);
        ChasingState = new ChasingState(this);
        AttackState = new AttackState(this);
        DieState = new DieState(this);
    }

    public virtual void Init(int level)
    {
        IsDie = false;
        stateMachine.ChangeState(IdleState);

        if (hpBar != null)
        {
            hpBar.Init(this);
        }

    }

    public virtual void Init(StageManager stageManager, Player player, int stageLevel)
    {
        IsDie = false;
        currentHealth = unitStat.maxHealth;
        Agent.speed = unitStat.moveSpeed;

        stateMachine.ChangeState(IdleState);
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public bool IsTargetInRange(float range)
    {
        if (Target != null)
        {
            return Vector3.Distance(transform.position, Target.position) <= range;
        }

        return false;
    }

    public void LookAtTarget()
    {
        if (Target != null)
        {
            Vector3 direction = (Target.position - transform.position).normalized;
            transform.forward = new Vector3(direction.x, 0f, direction.z);
        }
    }

    public virtual void Attack()
    {
        
    }

    protected virtual void Die()
    {
        stateMachine.ChangeState(DieState);
        IsDie = true;
    }

    public void DestoryObject()
    {
        Destroy(gameObject);
    }

    public bool IsTargetDie()
    {
        if (Target == null) return true;

        if (Target.TryGetComponent(out UnitBase target))
        {
            return target.IsDie;
        }

        return true;
    }
}