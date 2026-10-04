using UnityEngine;

public class EnemyActionSelector
{
    public BattleAction SelectAction(Combatant enemy, Combatant target)
    {
        if (!enemy.IsAlive) return null;
        if (enemy.IsAsleep) return null;
        if (enemy.Skills == null || enemy.Skills.Length == 0) return null;

        var skill = enemy.Skills[Random.Range(0, enemy.Skills.Length)];

        return new BattleAction(enemy, target, BattleActionType.Skill, skill);
    }
}