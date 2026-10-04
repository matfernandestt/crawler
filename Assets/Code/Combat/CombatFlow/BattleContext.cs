public class BattleContext
{
    public Combatant Player { get; }
    public Combatant Enemy { get; }

    public bool IsRunSuccessful { get; private set; }

    public bool IsFinished =>
        IsRunSuccessful ||
        !Player.IsAlive ||
        !Enemy.IsAlive;

    public BattleContext(
        Combatant player,
        Combatant enemy)
    {
        Player = player;
        Enemy = enemy;
    }

    public void RunFromBattle()
    {
        IsRunSuccessful = true;
    }

    public BattleOutcome GetOutcome()
    {
        if (IsRunSuccessful)
            return BattleOutcome.Run;

        if (Player.IsAlive && Enemy.IsAlive)
            return BattleOutcome.None;

        if (!Player.IsAlive)
            return BattleOutcome.Defeat;

        return BattleOutcome.Victory;
    }
}

public enum BattleOutcome
{
    None,
    Victory,
    Defeat,
    Run
}