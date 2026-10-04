using UnityEngine;

[CreateAssetMenu(fileName = "CounterSkillAction", menuName = "Data/Skill Actions/Counter")]
public class CounterSkillAction : BaseSkillAction
{
    [SerializeField] private float damageMultiplier = 1.5f;

    public override void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill)
    {
        if (user == null || !user.IsAlive) return;

        user.ActivateCounter(damageMultiplier);
    }
}