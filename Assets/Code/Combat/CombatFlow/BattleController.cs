using System;
using System.Collections;
using UnityEngine;

public class BattleController
{
    public BattleContext Context { get; }

    private readonly BattleActionResolver _actionResolver;
    private readonly EnemyActionSelector _enemyActionSelector;

    private BattleAction _selectedPlayerAction;
    
    public bool HasPlayerAction => _selectedPlayerAction != null;
    
    public event Action<BattleAction> OnPlayerActionSubmitted;
    
    public event Action<BattleAction> OnActionStarted;
    public event Action<BattleAction> OnActionResolved;
    public event Action<BattleOutcome> OnBattleFinished;
    
    private Func<BattleAction, IEnumerator> _presentAction;
    private Func<string, IEnumerator> _presentMessage;

    public BattleController(BattleContext context, Func<BattleAction, IEnumerator> presentAction, Func<string, IEnumerator> presentMessage)
    {
        Context = context;

        _actionResolver = new BattleActionResolver();
        _enemyActionSelector = new EnemyActionSelector();

        _presentAction = presentAction;
        _presentMessage = presentMessage;
    }

    public void SubmitPlayerAction(BattleAction action)
    {
        _selectedPlayerAction = action;
        OnPlayerActionSubmitted?.Invoke(action);
    }
    
    public BattleAction ConsumePlayerAction()
    {
        var action = _selectedPlayerAction;
        _selectedPlayerAction = null;

        return action;
    }
    
    public IEnumerator ProcessTurnStartEffects()
    {
        var player = Context.Player;
        var enemy = Context.Enemy;

        if (player.IsAlive)
        {
            var playerEffects = player.ProcessTurnEffects();

            if (playerEffects.healthRecovered > 0)
            {
                yield return PresentMessage($"{player.Name} recovered {playerEffects.healthRecovered} HP!");
            }

            if (playerEffects.manaRecovered > 0)
            {
                yield return PresentMessage($"{player.Name} recovered {playerEffects.manaRecovered} mana!");
            }
        }

        if (enemy.IsAlive)
        {
            var enemyEffects = enemy.ProcessTurnEffects();

            if (enemyEffects.healthRecovered > 0)
            {
                yield return PresentMessage($"{enemy.Name} recovered {enemyEffects.healthRecovered} HP!");
            }

            if (enemyEffects.manaRecovered > 0)
            {
                yield return PresentMessage($"{enemy.Name} recovered {enemyEffects.manaRecovered} mana!");
            }
        }
    }
    
    private IEnumerator PresentMessage(string message)
    {
        if (_presentMessage != null)
            yield return _presentMessage(message);
    }
    
    private bool CanAct(Combatant combatant)
    {
        return combatant.IsAlive && !combatant.IsAsleep;
    }

    public IEnumerator ResolveTurn()
    {
        if (_selectedPlayerAction == null)
        {
            Debug.LogError("No player action was submitted.");
            yield break;
        }

        var playerAction = ConsumePlayerAction();

        if (playerAction.Type == BattleActionType.Run)
        {
            OnActionStarted?.Invoke(playerAction);

            if (_presentAction != null)
                yield return _presentAction(playerAction);

            _actionResolver.Resolve(
                playerAction,
                Context);

            OnActionResolved?.Invoke(playerAction);

            var outcome = Context.GetOutcome();

            if (outcome != BattleOutcome.None)
            {
                OnBattleFinished?.Invoke(outcome);
            }

            yield break;
        }

        BattleAction enemyAction = null;
        if (Context.Enemy.CanAct)
        {
            enemyAction = _enemyActionSelector.SelectAction(Context.Enemy, Context.Player);

            if (enemyAction == null)
            {
                Debug.LogError($"Enemy {Context.Enemy.Name} has no available skills.");
                yield break;
            }
        }
        else
        {
            yield return PresentMessage($"{Context.Enemy.Name} is sleeping!");

            Context.Enemy.EndTurn();

            if (!Context.Enemy.IsAsleep)
            {
                yield return PresentMessage($"{Context.Enemy.Name} woke up!");
            }
        }

        var actionOrder = BattleTurnResolver.GetActionOrder(playerAction, enemyAction);

        foreach (var action in actionOrder)
        {
            if (Context.IsFinished)
                break;

            OnActionStarted?.Invoke(action);

            if (_presentAction != null)
                yield return _presentAction(action);

            if (Context.IsFinished)
                break;

            var result = _actionResolver.Resolve(action, Context);

            if (result == BattleActionResult.Missed)
            {
                yield return PresentMessage($"{action.User.Name} missed!");
            }
            else if (result == BattleActionResult.NotEnoughMana)
            {
                yield return PresentMessage($"{action.User.Name} doesn't have enough mana!");
            }

            OnActionResolved?.Invoke(action);
        }

        var battleOutcome = Context.GetOutcome();

        if (battleOutcome != BattleOutcome.None)
        {
            OnBattleFinished?.Invoke(battleOutcome);
        }
    }
}