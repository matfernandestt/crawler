public class BattleAction
{
    public Combatant User { get; }
    public Combatant Target { get; }

    public BattleActionType Type { get; }
    public SkillData Skill { get; }
    public ItemData Item { get; }
    
    public int Priority => Type == BattleActionType.Skill && Skill != null ? Skill.priority : 0;

    public BattleAction(Combatant user, Combatant target, BattleActionType type, SkillData skill = null, ItemData item = null)
    {
        User = user;
        Target = target;
        Type = type;
        Skill = skill;
        Item = item;
    }
}

public enum BattleActionType
{
    Skill,
    Item,
    Switch,
    Run
}