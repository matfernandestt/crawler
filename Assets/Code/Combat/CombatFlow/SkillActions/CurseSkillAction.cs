using UnityEngine;

[CreateAssetMenu(fileName = "CurseSkillAction", menuName = "Data/Skill Actions/Curse")]
public class CurseSkillAction : BaseSkillAction
{
    [SerializeField] private int defenseReduction = 10;
    [SerializeField] private int specialDefenseReduction = 10;
    [SerializeField] private int evasionReduction = 10;

    public override void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill)
    {
        if (target == null || !target.IsAlive)
            return;

        target.ReduceDefenses(defenseReduction, specialDefenseReduction, evasionReduction);
    }
}