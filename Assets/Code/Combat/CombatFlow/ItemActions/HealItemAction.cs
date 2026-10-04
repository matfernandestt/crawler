using UnityEngine;

[CreateAssetMenu(fileName = "HealItemAction", menuName = "Data/Item Actions/Heal")]
public class HealItemAction : BaseItemAction
{
    [SerializeField] private int totalHealing = 30;
    [SerializeField] private int durationTurns = 3;

    public override bool CanUse(BattleContext context, Combatant user, Combatant target, ItemData item)
    {
        return target != null && target.IsAlive && target.CurrentHealth < target.MaxHealth;
    }

    public override void Execute(BattleContext context, Combatant user, Combatant target, ItemData item)
    {
        if (target == null || !target.IsAlive) return;
        var amountPerTurn = Mathf.CeilToInt((float)totalHealing / durationTurns);
        target.ApplyHealthRegen(amountPerTurn, durationTurns);
    }
}