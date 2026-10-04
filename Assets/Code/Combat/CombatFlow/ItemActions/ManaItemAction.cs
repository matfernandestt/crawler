using UnityEngine;

[CreateAssetMenu(fileName = "ManaItemAction", menuName = "Data/Item Actions/Mana")]
public class ManaItemAction : BaseItemAction
{
    [SerializeField] private int totalManaRecovery = 10;
    [SerializeField] private int durationTurns = 3;

    public override bool CanUse(BattleContext context, Combatant user, Combatant target, ItemData item)
    {
        return target != null && target.IsAlive && target.CurrentMana < target.MaxMana;
    }

    public override void Execute(BattleContext context, Combatant user, Combatant target, ItemData item)
    {
        if (target == null || !target.IsAlive) return;
        var amountPerTurn = Mathf.CeilToInt((float)totalManaRecovery / durationTurns);
        target.ApplyManaRegen(amountPerTurn, durationTurns);
    }
}