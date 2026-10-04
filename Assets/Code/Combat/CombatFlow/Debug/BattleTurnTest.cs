using UnityEngine;

public class BattleTurnTest : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private SkillData playerSkill;
    [SerializeField] private SkillData enemySkill;

    private void Start()
    {
        var player = CombatantFactory.CreatePlayer(playerData);
        var enemy = CombatantFactory.CreateEnemy(enemyData);

        var playerAction = new BattleAction(player, enemy, BattleActionType.Skill, playerSkill);
        var enemyAction = new BattleAction(enemy, player, BattleActionType.Skill, enemySkill);

        var order = BattleTurnResolver.GetActionOrder(playerAction, enemyAction);

        Debug.Log($"Player Speed: {player.Speed}");
        Debug.Log($"Enemy Speed: {enemy.Speed}");
        Debug.Log($"First to act: {order[0].User.Name}");
    }
}