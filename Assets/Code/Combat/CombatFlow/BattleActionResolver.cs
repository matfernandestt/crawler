using UnityEngine;

public class BattleActionResolver
{
    public BattleActionResult Resolve(BattleAction action, BattleContext context)
    {
        if (action == null) return BattleActionResult.None;

        switch (action.Type)
        {
            case BattleActionType.Skill:
                return ResolveSkill(action, context);
            case BattleActionType.Item:
                ResolveItem(action, context);
                return BattleActionResult.Success;
            case BattleActionType.Switch:
                ResolveSwitch(action, context);
                return BattleActionResult.Success;
            case BattleActionType.Run:
                ResolveRun(action, context);
                return BattleActionResult.Success;
        }
        return BattleActionResult.None;
    }

    private BattleActionResult ResolveSkill(BattleAction action, BattleContext context)
    {
        if (action.Skill == null) return BattleActionResult.None;
        if (action.Skill.action == null) return BattleActionResult.None;
        if (!action.User.CanUseSkill(action.Skill.cost)) return BattleActionResult.NotEnoughMana;

        var effectiveAccuracy = Mathf.Clamp(action.Skill.accuracy - action.Target.Evasion, 0, 100);
        var accuracyRoll = Random.Range(0, 100);
        if (accuracyRoll >= effectiveAccuracy)
            return BattleActionResult.Missed;

        action.User.TrySpendMana(action.Skill.cost);
        action.Skill.action.Execute(context, action.User, action.Target, action.Skill);

        return BattleActionResult.Success;
    }

    private void ResolveItem(BattleAction action, BattleContext context)
    {
        if (action.Item == null) return;
        if (action.Item.action == null) return;

        action.Item.action.Execute(context, action.User, action.Target, action.Item);
    }

    private void ResolveSwitch(BattleAction action, BattleContext context)
    {
        // pokémon tem isso aqui, então vejo depois
    }

    private void ResolveRun(BattleAction action, BattleContext context)
    {
        context.RunFromBattle();
    }
}

public enum BattleActionResult
{
    None,
    Success,
    Missed,
    NotEnoughMana
}