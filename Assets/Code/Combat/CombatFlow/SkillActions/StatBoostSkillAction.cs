using UnityEngine;

[CreateAssetMenu(fileName = "StatBoostSkillAction", menuName = "Data/Skill Actions/Stat Boost")]
public class StatBoostSkillAction : BaseSkillAction
{
    [Header("Offensive")]
    [SerializeField] private int attackBoost = 10;
    [SerializeField] private int specialAttackBoost = 10;
    [SerializeField] private int accuracyBoost = 10;

    [Header("Defensive")]
    [SerializeField] private int defenseBoost = 10;
    [SerializeField] private int specialDefenseBoost = 10;
    [SerializeField] private int evasionBoost = 10;

    [Header("Other")]
    [SerializeField] private int speedBoost = 10;

    public override void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill)
    {
        if (user == null || !user.IsAlive)
            return;

        user.IncreaseAllStats(attackBoost, defenseBoost, specialAttackBoost, specialDefenseBoost, evasionBoost, accuracyBoost, speedBoost);
    }
}