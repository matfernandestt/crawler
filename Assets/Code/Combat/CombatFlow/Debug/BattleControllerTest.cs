using System.Collections;
using UnityEngine;

public class BattleControllerTest : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private SkillData playerSkill;

    private void Start()
    {
        var player = CombatantFactory.CreatePlayer(playerData);
        var enemy = CombatantFactory.CreateEnemy(enemyData);

        var context = new BattleContext(player, enemy);
        var controller = new BattleController(context, PresentAction, PresentMessage);

        enemy.OnTakeDamage += OnEnemyTakeDamage;
        enemy.OnFainted += OnEnemyFainted;

        var playerAction = new BattleAction(player, enemy, BattleActionType.Skill, playerSkill);
        controller.SubmitPlayerAction(playerAction);

        StartCoroutine(controller.ResolveTurn());
    }

    private IEnumerator PresentMessage(string arg)
    {
        yield return null;
    }

    private IEnumerator PresentAction(BattleAction action)
    {
        yield return null;
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