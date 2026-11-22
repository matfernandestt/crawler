using System;
using System.Collections.Generic;
using UnityEngine;

public class Combatant
{
    public string Name { get; }

    public int MaxHealth { get; }
    public int CurrentHealth { get; private set; }

    public int MaxMana { get; }
    public int CurrentMana { get; private set; }

    public int Attack { get; }
    public int Defense { get; }

    public int SpecialAttack { get; }
    public int SpecialDefense { get; }

    public int Evasion { get; }
    public int Accuracy { get; }
    public int Speed { get; }

    public SkillData[] Skills { get; }
    
    public int SleepTurns { get; private set; }
    public int SpeedModifier { get; private set; }

    public int EffectiveSpeed => Mathf.Max(0, Speed + SpeedModifier);
    public bool IsAsleep => SleepTurns > 0;
    public bool CanAct => IsAlive && !IsAsleep;
    public bool IsCountering { get; private set; }
    public float CounterDamageMultiplier { get; private set; }
    
    public int HealthRegenAmount { get; private set; }
    public int HealthRegenTurns { get; private set; }

    public int ManaRegenAmount { get; private set; }
    public int ManaRegenTurns { get; private set; }

    public bool IsAlive => CurrentHealth > 0;
    
    public int DefenseModifier { get; private set; }
    public int SpecialDefenseModifier { get; private set; }
    public int EvasionModifier { get; private set; }

    public int EffectiveDefense => Mathf.Max(0, Defense + DefenseModifier);
    public int EffectiveSpecialDefense => Mathf.Max(0, SpecialDefense + SpecialDefenseModifier);
    public int EffectiveEvasion => Mathf.Clamp(Evasion + EvasionModifier, 0, 100);
    
    public int AttackModifier { get; private set; }
    public int SpecialAttackModifier { get; private set; }
    public int AccuracyModifier { get; private set; }

    public int EffectiveAttack => Mathf.Max(0, Attack + AttackModifier);
    public int EffectiveSpecialAttack => Mathf.Max(0, SpecialAttack + SpecialAttackModifier);
    public int EffectiveAccuracy => Mathf.Clamp(Accuracy + AccuracyModifier, 0, 100);
    
    private readonly HashSet<SkillEffectType> _immunities = new();

    public ItemData[] PossibleDrops;

    public event Action<Combatant, int> OnTakeDamage;
    public event Action<Combatant, int> OnChangeMana;
    public event Action<Combatant, int> OnHeal;
    public event Action<Combatant> OnFainted;
    public event Action<Combatant> OnStatsLowered;

    public Combatant(Sprite sprite, string name, int maxHealth, int health, int maxMana, int mana, int attack, int defense, int specialAttack, int specialDefense, int evasion, int accuracy, int speed, SkillData[] skills, SkillEffectType[] immunities = null, ItemData[] possibleDrops = null)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = health;
        MaxMana = maxMana;
        CurrentMana = mana;
        Attack = attack;
        Defense = defense;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        Evasion = evasion;
        Accuracy = accuracy;
        Speed = speed;
        Skills = skills;

        if (immunities != null)
        {
            foreach (var immunity in immunities)
            {
                _immunities.Add(immunity);
            }
        }

        PossibleDrops = possibleDrops;
    }
    
    public bool IsImmuneTo(SkillEffectType effect)
    {
        return _immunities.Contains(effect);
    }
    
    public bool CanHeal(int amount)
    {
        return IsAlive && amount > 0 && CurrentHealth < MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive)
            return;

        damage = Mathf.Max(0, damage);

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

        OnTakeDamage?.Invoke(this, damage);

        if (CurrentHealth == 0)
            OnFainted?.Invoke(this);
    }

    public int Heal(int amount)
    {
        if (!IsAlive)
            return 0;

        amount = Mathf.Max(0, amount);

        var previousHealth = CurrentHealth;

        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);

        var healedAmount = CurrentHealth - previousHealth;

        if (healedAmount > 0)
        {
            OnHeal?.Invoke(this, healedAmount);
        }

        return healedAmount;
    }
    
    public int RestoreMana(int amount)
    {
        if (!IsAlive) return 0;

        amount = Mathf.Max(0, amount);

        var previousMana = CurrentMana;
        CurrentMana = Mathf.Min(MaxMana, CurrentMana + amount);

        OnChangeMana?.Invoke(this, previousMana);
        return CurrentMana - previousMana;
    }
    
    public bool CanRestoreMana(int amount)
    {
        return IsAlive && amount > 0 && CurrentMana < MaxMana;
    }
    
    public bool CanUseSkill(int manaCost)
    {
        return IsAlive && manaCost >= 0 && CurrentMana >= manaCost;
    }

    public bool TrySpendMana(int amount)
    {
        if (amount < 0 || CurrentMana < amount) return false;

        CurrentMana -= amount;
        OnChangeMana?.Invoke(this, CurrentMana);
        return true;
    }
    
    public void ActivateCounter(float damageMultiplier)
    {
        if (!IsAlive)
            return;

        IsCountering = true;
        CounterDamageMultiplier = Mathf.Max(0f, damageMultiplier);
    }

    public void ClearCounter()
    {
        IsCountering = false;
        CounterDamageMultiplier = 0f;
    }
    
    public void ReduceDefenses(int defense, int specialDefense, int evasion)
    {
        if (!IsAlive)
            return;

        DefenseModifier -= Mathf.Max(0, defense);
        SpecialDefenseModifier -= Mathf.Max(0, specialDefense);
        EvasionModifier -= Mathf.Max(0, evasion);
        OnStatsLowered?.Invoke(this);
    }
    
    public void IncreaseAllStats(int attack, int defense, int specialAttack, int specialDefense, int evasion, int accuracy, int speed)
    {
        if (!IsAlive)
            return;

        AttackModifier += Mathf.Max(0, attack);
        DefenseModifier += Mathf.Max(0, defense);
        SpecialAttackModifier += Mathf.Max(0, specialAttack);
        SpecialDefenseModifier += Mathf.Max(0, specialDefense);
        EvasionModifier += Mathf.Max(0, evasion);
        AccuracyModifier += Mathf.Max(0, accuracy);
        SpeedModifier += Mathf.Max(0, speed);
    }
    
    public void ApplySleep(int turns)
    {
        if (!IsAlive) return;

        SleepTurns = Mathf.Max(SleepTurns, turns);
    }

    public void IncreaseSpeed(int amount)
    {
        if (!IsAlive) return;

        SpeedModifier += amount;
    }
    
    public void ApplyHealthRegen(int amountPerTurn, int turns)
    {
        if (!IsAlive)
            return;

        HealthRegenAmount = Mathf.Max(0, amountPerTurn);
        HealthRegenTurns = Mathf.Max(0, turns);
    }

    public void ApplyManaRegen(int amountPerTurn, int turns)
    {
        if (!IsAlive)
            return;

        ManaRegenAmount = Mathf.Max(0, amountPerTurn);
        ManaRegenTurns = Mathf.Max(0, turns);
    }
    
    public (int healthRecovered, int manaRecovered) ProcessTurnEffects()
    {
        if (!IsAlive)
            return (0, 0);

        var healthRecovered = 0;
        var manaRecovered = 0;

        if (HealthRegenTurns > 0)
        {
            healthRecovered = Heal(HealthRegenAmount);

            HealthRegenTurns--;

            if (HealthRegenTurns == 0)
                HealthRegenAmount = 0;
        }

        if (ManaRegenTurns > 0)
        {
            manaRecovered = RestoreMana(ManaRegenAmount);

            ManaRegenTurns--;

            if (ManaRegenTurns == 0)
                ManaRegenAmount = 0;
        }

        return (healthRecovered, manaRecovered);
    }
    
    public void EndTurn()
    {
        if (SleepTurns > 0)
        {
            SleepTurns--;
        }
    }
}