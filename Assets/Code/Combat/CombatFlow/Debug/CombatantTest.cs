using UnityEngine;

public class CombatantTest : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private SkillData testSkill;

    private void Start()
    {
        Combatant player = CombatantFactory.CreatePlayer(playerData);
        Combatant enemy = CombatantFactory.CreateEnemy(enemyData);

        var battle = new BattleContext(player, enemy);
        var action = new BattleAction(player, enemy, BattleActionType.Skill, testSkill);
        var resolver = new BattleActionResolver();

        enemy.OnTakeDamage += OnEnemyTakeDamage;
        enemy.OnFainted += OnEnemyFainted;

        Debug.Log($"{player.Name} uses {testSkill.skillName}!");

        resolver.Resolve(action, battle);

        Debug.Log($"{enemy.Name} now has {enemy.CurrentHealth} HP");
    }

    private void OnEnemyTakeDamage(Combatant combatant, int damage)
    {
        Debug.Log($"{combatant.Name} took {damage} damage.");
    }

    private void OnEnemyFainted(Combatant combatant)
    {
        Debug.Log($"{combatant.Name} fainted.");
    }
}