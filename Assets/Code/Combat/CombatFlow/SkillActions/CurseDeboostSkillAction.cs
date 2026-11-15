using UnityEngine;

[CreateAssetMenu(fileName = "CurseDeboostSkillAction", menuName = "Data/Skill Actions/Speed Boost")]
public class CurseDeboostSkillAction : BaseSkillAction
{
    [SerializeField] private int speedIncrease = 10;

    public override void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill)
    {
        if (user == null || !user.IsAlive) return;
        user.IncreaseSpeed(speedIncrease);
    }
}