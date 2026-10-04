using UnityEngine;

[CreateAssetMenu(fileName = "SleepSkillAction", menuName = "Data/Skill Actions/Sleep")]
public class SleepSkillAction : BaseSkillAction
{
    [SerializeField] private int turns = 3;

    public override void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill)
    {
        if (target == null || !target.IsAlive) return;

        target.ApplySleep(turns);
    }
}