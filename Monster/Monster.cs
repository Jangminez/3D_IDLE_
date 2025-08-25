using UnityEngine;

public class Monster : UnitBase, IDamgeable
{
    private StageManager stageManager;

    [SerializeField] private MonsterStatSO monsterStatSO;
    [SerializeField] private int currentStage = 1;

    private int dropGold;
    private int dropExp;

    Player player;

    public override void Init(StageManager stageManager, Player player, int stage)
    {
        this.stageManager = stageManager;
        this.player = player;
        Target = player.transform;

        currentStage = stage;

        statSO = monsterStatSO;
        unitStat = monsterStatSO.GetStatByLevel(currentStage);
        currentHealth = unitStat.maxHealth;
        
        dropGold = monsterStatSO.GetGoldDrop(currentStage);
        dropExp = monsterStatSO.GetExpDrop(currentStage);

        base.Init(stage);
    }

    public override void Attack()
    {
        base.Attack();

        if (Target.TryGetComponent(out IDamgeable damgeable))
        {
            damgeable.TakeDamage(unitStat.attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - unitStat.defense > 0 ? damage - unitStat.defense : 1;
        currentHealth -= finalDamage;

        onHealthChanged?.Invoke(currentHealth, unitStat.maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    protected override void Die()
    {
        base.Die();

        stageManager.RemoveMonster(this);
        player.GetReward(dropGold, dropExp);
    }
}
