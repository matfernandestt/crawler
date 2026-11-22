public static class CombatantFactory
{
    public static Combatant CreatePlayer(PlayerData data)
    {
        var player = PlayerReferences.Instance.attributes;
        return new Combatant(
            data.sprite,
            data.playerName,
            player.GetData.health,
            player.CurrentHealth,
            player.GetData.mana,
            player.CurrentMana,
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
            data.health,
            data.mana,
            data.mana,
            data.attack,
            data.defense,
            data.specialAttack,
            data.specialDefense,
            data.evasion,
            data.accuracy,
            data.speed,
            data.skills,
            data.immunities,
            data.possibleDropsOnDefeat);
    }
}