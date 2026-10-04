using UnityEngine;

public abstract class BaseItemAction : ScriptableObject
{
    public abstract bool CanUse(BattleContext context, Combatant user, Combatant target, ItemData item);
    
    public abstract void Execute(BattleContext context, Combatant user, Combatant target, ItemData item);
}