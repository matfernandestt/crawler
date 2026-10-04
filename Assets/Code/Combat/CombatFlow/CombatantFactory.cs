public static class CombatantFactory
{
    public static Combatant CreatePlayer(PlayerData data)
    {
        return new Combatant(
            data.sprite,
            data.playerName,
            data.health,
            data.mana,
            data.attack,
            data.defense,
            data.specialAttack,
            data.specialDefense,
            data.evasion,
            data.accuracy,
            data.speed,
            data.skills);
    }

    public static Combatant CreateEnemy(EnemyData data)
    {
        return new Combatant(
            data.icon,
            data.enemyName,
            data.health,
            data.mana,
            data.attack,
            data.defense,
            data.specialAttack,
            data.specialDefense,
            data.evasion,
            data.accuracy,
            data.speed,
            data.skills);
    }
}