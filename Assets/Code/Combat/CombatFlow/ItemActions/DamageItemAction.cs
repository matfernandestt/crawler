using UnityEngine;

[CreateAssetMenu(fileName = "DamageItemAction", menuName = "Data/Item Actions/Damage")]
public class DamageItemAction : BaseItemAction
{
    [SerializeField] private int damage = 30;

    public override bool CanUse(BattleContext context, Combatant user, Combatant target, ItemData item)
    {
        return target != null && target.IsAlive;
    }

    public override void Execute(BattleContext context, Combatant user, Combatant target, ItemData item)
    {
        if (target == null || !target.IsAlive) return;
        target.TakeDamage(damage);
    }
}