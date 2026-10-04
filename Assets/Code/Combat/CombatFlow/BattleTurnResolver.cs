public static class BattleTurnResolver
{
    public static BattleAction[] GetActionOrder(BattleAction playerAction, BattleAction enemyAction)
    {
        if (playerAction == null)
            return new[] { enemyAction };

        if (enemyAction == null)
            return new[] { playerAction };

        if (playerAction.Priority > enemyAction.Priority)
        {
            return new[]
            {
                playerAction,
                enemyAction
            };
        }

        if (enemyAction.Priority > playerAction.Priority)
        {
            return new[]
            {
                enemyAction,
                playerAction
            };
        }

        if (playerAction.User.EffectiveSpeed >= enemyAction.User.EffectiveSpeed)
        {
            return new[]
            {
                playerAction,
                enemyAction
            };
        }

        return new[]
        {
            enemyAction,
            playerAction
        };
    }
}