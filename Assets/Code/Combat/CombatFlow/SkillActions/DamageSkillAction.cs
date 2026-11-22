using UnityEngine;

[CreateAssetMenu(fileName = "DamageSkillAction", menuName = "Data/Skill Actions/Damage")]
public class DamageSkillAction : BaseSkillAction
{
    public override void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill)
    {
        if (user == null || !user.IsAlive)
            return;

        if (target == null || !target.IsAlive)
            return;

        int attack;
        int defense;

        switch (skill.damageType)
        {
            case SkillDamageType.Physical:
                attack = user.EffectiveAttack;
                defense = target.EffectiveDefense;
                break;
            case SkillDamageType.Special:
                attack = user.EffectiveSpecialAttack;
                defense = target.EffectiveSpecialDefense;
                break;
            default:
                return;
        }

        var damage = Mathf.RoundToInt(skill.power * ((float)attack / Mathf.Max(1, defense)));
        damage = Mathf.Max(1, damage);

        var previousHealth = target.CurrentHealth;

        target.TakeDamage(damage);

        var damageDealt = previousHealth - target.CurrentHealth;

        if (target.IsAlive && target.IsCountering)
        {
            var counterDamage = Mathf.RoundToInt(damageDealt * target.CounterDamageMultiplier);
            user.TakeDamage(counterDamage);
            target.ClearCounter();
        }
    }
}