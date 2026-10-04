using UnityEngine;

[CreateAssetMenu(fileName = "DamageSkillAction", menuName = "Data/Skill Actions/Damage")]
public class DamageSkillAction : BaseSkillAction
{
    public override void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill)
    {
        if (target == null || !target.IsAlive) return;
        
        var previousHealth = target.CurrentHealth;

        target.TakeDamage(skill.power);

        var damageDealt = previousHealth - target.CurrentHealth;

        if (target.IsAlive && target.IsCountering)
        {
            var counterDamage = Mathf.RoundToInt(damageDealt * target.CounterDamageMultiplier);
            user.TakeDamage(counterDamage);
            target.ClearCounter();
        }
    }
}