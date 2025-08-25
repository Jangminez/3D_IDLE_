using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : UnitBase, IDamgeable
{
    [SerializeField] PlayerStatSO playerStatSO;
    private List<Monster> monsters = new List<Monster>();

    [SerializeField] private LayerMask monsterLayer;
    private int currentLevel = 1;
    private int currentExp = 0;
    private int requiredExp;
    private int currentGold;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0f, TotalMaxHealth);
            onHealthChanged?.Invoke(currentHealth, TotalMaxHealth);
        }
    }

    [Header("Equipment Bonus Stats")]
    [SerializeField] private float bonusHealth = 0f;
    [SerializeField] private float bonusAttackDamage = 0f;
    [SerializeField] private float bonusDefense = 0f;
    [SerializeField] private float bonusAttackSpeed = 0f;
    [SerializeField] private float bonusSpeed = 0f;

    [Header("Stat Multiplier")]
    [SerializeField] private float attackDamageMultiplier = 1f;
    [SerializeField] private float defenseMultiplier = 1f;
    [SerializeField] private float attackSpeedMultiplier = 1f;
    [SerializeField] private float speedMultiplier = 1f;

    public float TotalMaxHealth => Stat.maxHealth + bonusHealth;
    public float TotalAttackDamage => (Stat.attackDamage + bonusAttackDamage) * attackDamageMultiplier;
    public float TotalDefense => (Stat.defense + bonusDefense) * defenseMultiplier;
    public float TotalAttackSpeed => (Stat.attackSpeed + bonusAttackSpeed) * attackSpeedMultiplier;
    public float TotalSpeed => (Stat.moveSpeed + bonusSpeed) * speedMultiplier;

    private PlayerEventHandler playerEvent;
    public PlayerEventHandler Events { get => playerEvent; }

    protected override void Awake()
    {
        base.Awake();

        playerEvent = new PlayerEventHandler();
    }

    public override void Init(int level)
    {
        currentLevel = level;

        statSO = playerStatSO;
        requiredExp = playerStatSO.GetRequiredExp(currentLevel);
        unitStat = playerStatSO.GetStatByLevel(currentLevel);

        currentHealth = TotalMaxHealth;

        base.Init(level);

        InitRaiseEvents();

        GetReward(1000000, 0);
    }

    protected override void Update()
    {
        base.Update();

        if (Target == null || IsTargetDie() && monsters != null)
        {
            FindNextTarget();
        }
    }

    void InitRaiseEvents()
    {
        playerEvent.RaiseLevelChanged(currentLevel);
        playerEvent.RaiseGoldChanged(currentGold);
        playerEvent.RaiseExpChanged(currentExp, requiredExp);
    }

    public void StartStage(Vector3 startPosition)
    {
        Agent.Warp(startPosition);
        CurrentHealth = TotalMaxHealth;
    }

    public override void Attack()
    {
        base.Attack();

        if (monsters == null) return;

        foreach (var monster in monsters.ToList())
        {
            if (monster == null) continue;

            if (Vector3.Distance(transform.position, monster.transform.position) <= unitStat.attackRange)
            {
                if (monster.TryGetComponent(out IDamgeable damgeable))
                {
                    damgeable.TakeDamage(TotalAttackDamage);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - TotalDefense > 0 ? damage - TotalDefense : 1;
        CurrentHealth -= finalDamage;

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Respawn()
    {
        CurrentHealth = TotalMaxHealth;
        Agent.isStopped = false;

        GameManager.Instance.RestartStage();
    }

    public void SetMonsters(List<Monster> monsters)
    {
        this.monsters = monsters;
    }

    public void GetReward(int gold, int exp)
    {
        currentGold += gold;
        currentExp += exp;

        playerEvent.RaiseGoldChanged(currentGold);
        playerEvent.RaiseExpChanged(currentExp, requiredExp);

        LevelUp();
    }

    private void LevelUp()
    {
        while (currentExp >= requiredExp)
        {
            currentLevel++;
            currentExp -= requiredExp;

            unitStat = playerStatSO.GetStatByLevel(currentLevel);
            requiredExp = playerStatSO.GetRequiredExp(currentLevel);
            CurrentHealth = unitStat.maxHealth;

            playerEvent.RaiseLevelChanged(currentLevel);
            playerEvent.RaiseExpChanged(currentExp, requiredExp);
        }
    }

    private void FindNextTarget()
    {
        monsters.RemoveAll(m => m == null);

        float minDistance = float.MaxValue;
        Transform nearMonster = null;

        foreach (var monster in monsters)
        {
            if (monster == null)
            {
                monsters.Remove(monster);
                continue;
            }

            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearMonster = monster.transform;
            }
        }

        if (nearMonster != null)
            Target = nearMonster;
    }

    public bool UseGold(int needGold)
    {
        if (currentGold >= needGold)
        {
            currentGold -= needGold;
            Events.RaiseGoldChanged(currentGold);

            return true;
        }

        return false;
    }

    public void CalculateBonusStat(List<ItemInstance> equippedItems)
    {
        ResetBonusStats();

        foreach (var item in equippedItems)
        {
            ApplyBonusStat(item.ItemData.statType, item.GetTotalStatValue());
        }
    }

    private void ResetBonusStats()
    {
        bonusAttackDamage = 0f;
        bonusAttackSpeed = 0f;
        bonusDefense = 0f;
        bonusSpeed = 0f;
        bonusHealth = 0f;
    }

    private void ApplyBonusStat(StatType type, float value)
    {
        switch (type)
        {
            case StatType.Attack:
                bonusAttackDamage += value;
                break;
            case StatType.AttackSpeed:
                bonusAttackSpeed += value;
                break;
            case StatType.Defense:
                bonusDefense += value;
                break;
            case StatType.Speed:
                bonusSpeed += value;
                break;
            case StatType.Health:
                bonusHealth += value;
                break;
        }
    }

    public void ApplyItemEffect(ItemInstance item)
    {
        if (item == null) return;

        ItemData itemData = item.ItemData;

        if (itemData.isConsumable)
        {
            if (itemData.isTemporary)
            {
                StartCoroutine(ApplyTemporaryEffect(itemData.statType, itemData.baseValue, itemData.duration));
            }
            else
            {
                float healAmount = TotalMaxHealth * itemData.baseValue;
                CurrentHealth += healAmount;
            }
        }
    }

    private IEnumerator ApplyTemporaryEffect(StatType type, float multiplier, float duration)
    {
        ApplyMultiplier(type, multiplier, true);
        yield return new WaitForSeconds(duration);
        ApplyMultiplier(type, multiplier, false);
    }

    private void ApplyMultiplier(StatType type, float multiplier, bool isApply)
    {
        switch (type)
        {
            case StatType.Attack:
                attackDamageMultiplier = isApply ? attackDamageMultiplier + multiplier : attackDamageMultiplier - multiplier;
                break;
            case StatType.AttackSpeed:
                attackSpeedMultiplier = isApply ? attackSpeedMultiplier + multiplier : attackSpeedMultiplier - multiplier;
                break;
            case StatType.Defense:
                defenseMultiplier = isApply ? defenseMultiplier + multiplier : defenseMultiplier - multiplier;
                break;
            case StatType.Speed:
                speedMultiplier = isApply ? speedMultiplier + multiplier : speedMultiplier - multiplier;
                break;
        }
    }
}
