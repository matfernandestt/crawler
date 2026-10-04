public class TurnOrderResolver
{
    public Combatant DetermineFirst(Combatant player, Combatant enemy)
    {
        if (player.Speed >= enemy.Speed)
            return player;

        return enemy;
    }
}