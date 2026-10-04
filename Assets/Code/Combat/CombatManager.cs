using System;
using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    
    public BattleMessagePresenter messages;
    
    public bool InCombat { get; private set; }
    public EnemyData CurrentEnemy { get; private set; }
    
    [SerializeField] private GameObject combatButtons;
    [SerializeField] private EnemyDatabase database;
    [SerializeField] private EnemyVisual enemyVisual;
    [SerializeField] private CombatActions actions;
    [SerializeField] private SkillWindow skillWindow;
    [SerializeField] private ItemWindow itemWindow;
    
    private BattleController _battleController;
    private BattleContext _battleContext;

    public Action<BattleContext> onSetupBattle;
    public Action onBattleEnded;

    private void Awake()
    {
        Instance = this;
        
        combatButtons.SetActive(false);
        enemyVisual.gameObject.SetActive(false);
        skillWindow.gameObject.SetActive(false);
        itemWindow.gameObject.SetActive(false);
        
        actions.OnActionSelected += OnActionSelected;
        skillWindow.OnSkillSelected += OnSkillSelected;
        itemWindow.OnItemSelected += OnItemSelected;
        
        skillWindow.onBack += OnCloseWindow;
        itemWindow.onBack += OnCloseWindow;
    }

    private void OnDestroy()
    {
        actions.OnActionSelected -= OnActionSelected;
        skillWindow.OnSkillSelected -= OnSkillSelected;
        itemWindow.OnItemSelected -= OnItemSelected;
        
        skillWindow.onBack -= OnCloseWindow;
        itemWindow.onBack -= OnCloseWindow;
    }
    
    private void OnActionSelected(BattleActionType actionType)
    {
        switch (actionType)
        {
            case BattleActionType.Skill:
                OpenSkillSelection();
                break;
            case BattleActionType.Item:
                OpenItemSelection();
                break;
            case BattleActionType.Switch:
                Debug.Log("Switch selected.");
                break;
            case BattleActionType.Run:
                SubmitSimpleAction(BattleActionType.Run);
                break;
        }
    }
    
    private void OpenSkillSelection()
    {
        actions.SetAllButtonsInteractability(false);
        skillWindow.gameObject.SetActive(true);
    }
    
    private void OpenItemSelection()
    {
        actions.SetAllButtonsInteractability(false);

        itemWindow.SetupItems(PlayerInventory.Instance.Inventory);
        itemWindow.gameObject.SetActive(true);
    }
    
    private void OnItemSelected(ItemData item)
    {
        itemWindow.gameObject.SetActive(false);
        SubmitItemAction(item);
    }
    
    private void SubmitSimpleAction(BattleActionType actionType)
    {
        var player = _battleContext.Player;
        var action = new BattleAction(player, null, actionType);

        _battleController.SubmitPlayerAction(action);

        StartCoroutine(ResolveBattleTurn());
    }
    
    private void SubmitItemAction(ItemData item)
    {
        if (item == null)
            return;

        var player = _battleContext.Player;

        if (item.action == null)
            return;

        var target = item.target == ItemTarget.Self
            ? player
            : _battleContext.Enemy;

        if (!item.action.CanUse(_battleContext, player, target, item))
        {
            ShowInvalidItemMessage(item);
            return;
        }

        if (!PlayerInventory.Instance.Inventory.TryUse(item))
        {
            Debug.Log("No items remaining.");
            return;
        }

        var action = new BattleAction(player, target, BattleActionType.Item,
            item: item);

        _battleController.SubmitPlayerAction(action);

        StartCoroutine(ResolveBattleTurn());
    }
    
    private void OnSkillSelected(SkillData skill)
    {
        skillWindow.gameObject.SetActive(false);

        var player = _battleContext.Player;

        if (!player.CanUseSkill(skill.cost))
        {
            ShowInvalidSkillMessage(skill);
            return;
        }
        var enemy = _battleContext.Enemy;
        
        var action = new BattleAction(player, enemy, BattleActionType.Skill, skill);
        
        _battleController.SubmitPlayerAction(action);

        StartCoroutine(ResolveBattleTurn());
    }
    
    private IEnumerator PresentMessage(string message)
    {
        var confirmed = false;
        messages.SetMessage(message, () => confirmed = true);
        while (!confirmed) yield return null;
    }
    
    private void ShowInvalidSkillMessage(SkillData skill)
    {
        messages.SetMessage($"Not enough mana to use {skill.skillName}!", OpenSkillSelection);
    }
    
    private void ShowInvalidItemMessage(ItemData item)
    {
        messages.SetMessage($"{item.itemName} can't be used right now!", OpenItemSelection);
    }
    
    private IEnumerator ResolveBattleTurn()
    {
        actions.SetAllButtonsInteractability(false);

        yield return _battleController.ResolveTurn();

        if (!_battleContext.IsFinished)
        {
            yield return _battleController.ProcessTurnStartEffects();

            if (!_battleContext.IsFinished)
            {
                actions.SetAllButtonsInteractability(true);
            }
        }
    }

    public void EnterCombat()
    {
        InCombat = true;
        PlayerReferences.Instance.input.SetBlockMovement(true);
        TransitionManager.Instance.Fade(OnFadedToCombat, OnCompletedFadeToCombat);
        actions.SetAllButtonsInteractability(false);
    }

    private void ExitCombat()
    {
        InCombat = false;
        TransitionManager.Instance.Fade(() =>
        {
            combatButtons.SetActive(false);
            enemyVisual.gameObject.SetActive(false);
            
        }, SuccessfullyExitedCombat);
    }

    private void OnFadedToCombat()
    {
        combatButtons.SetActive(true);
        
        SetupCombat();

        var playerTransform = PlayerReferences.Instance.transform;
        enemyVisual.gameObject.SetActive(true);
        enemyVisual.transform.position = playerTransform.position + (playerTransform.forward * 10f) + Vector3.up * 2f;
        enemyVisual.transform.forward =  playerTransform.forward;
    }

    private void SetupCombat()
    {
        CurrentEnemy = database.GetRandomEnemy();

        var playerData = PlayerReferences.Instance.attributes.GetData;
        var playerCombatant = CombatantFactory.CreatePlayer(playerData);

        var enemyCombatant = CombatantFactory.CreateEnemy(CurrentEnemy);

        _battleContext = new BattleContext(playerCombatant, enemyCombatant);
        _battleController = new BattleController(_battleContext, PresentAction, PresentMessage);

        playerCombatant.OnTakeDamage += OnCombatantTakeDamage;
        playerCombatant.OnHeal += OnCombatantHeal;
        playerCombatant.OnFainted += OnCombatantFainted;

        enemyCombatant.OnTakeDamage += OnCombatantTakeDamage;
        enemyCombatant.OnHeal += OnCombatantHeal;
        enemyCombatant.OnFainted += OnCombatantFainted;

        enemyVisual.SetupEnemy(CurrentEnemy);
        skillWindow.SetupSkills(playerCombatant.Skills);
        
        _battleController.OnActionStarted += OnActionStarted;
        _battleController.OnActionResolved += OnActionResolved;
        _battleController.OnBattleFinished += OnBattleFinished;
        
        onSetupBattle?.Invoke(_battleContext);
    }
    
    private void UnsubscribeFromBattle()
    {
        if (_battleController != null)
        {
            _battleController.OnActionStarted -= OnActionStarted;
            _battleController.OnActionResolved -= OnActionResolved;
            _battleController.OnBattleFinished -= OnBattleFinished;
        }
        
        if (_battleContext != null)
        {
            _battleContext.Player.OnTakeDamage -= OnCombatantTakeDamage;
            _battleContext.Player.OnHeal -= OnCombatantHeal;
            _battleContext.Player.OnFainted -= OnCombatantFainted;

            _battleContext.Enemy.OnTakeDamage -= OnCombatantTakeDamage;
            _battleContext.Enemy.OnHeal -= OnCombatantHeal;
            _battleContext.Enemy.OnFainted -= OnCombatantFainted;
        }
    }

    private void OnCompletedFadeToCombat()
    {
        actions.SetAllButtonsInteractability(true);
        Debug.Log("Combat ready.");
    }
    
    private IEnumerator PresentAction(BattleAction action)
    {
        var confirmed = false;

        var message = action.Type switch
        {
            BattleActionType.Skill => $"{action.User.Name} used {action.Skill.skillName}!",
            BattleActionType.Item => $"{action.User.Name} used {action.Item.itemName}!",
            BattleActionType.Run => $"{action.User.Name} tried to run away!",
            _ => $"{action.User.Name} performed an action."
        };

        messages.SetMessage(message, () => confirmed = true);
        while (!confirmed) yield return null;

        if (action.Type == BattleActionType.Skill)
        {
            if (action.User == _battleContext.Player)
            {
                PlayerReferences.Instance.animations.SetAttack();
            }
            else
            {
                Debug.Log("Enemy attack animation.");
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    public void OnCloseWindow()
    {
        skillWindow.gameObject.SetActive(false);
        itemWindow.gameObject.SetActive(false);
        actions.SetAllButtonsInteractability(true);
    }

    private void SuccessfullyExitedCombat()
    {
        UnsubscribeFromBattle();

        _battleController = null;
        _battleContext = null;

        PlayerReferences.Instance.input.SetBlockMovement(false);
        onBattleEnded?.Invoke();
    }
    
    private void OnActionStarted(BattleAction action)
    {
    }

    private void OnActionResolved(BattleAction action)
    {
    }

    private void OnBattleFinished(BattleOutcome outcome)
    {
        actions.SetAllButtonsInteractability(false);

        switch (outcome)
        {
            case BattleOutcome.Victory:
                messages.SetMessage("Enemy defeated!", ExitCombat);
                break;

            case BattleOutcome.Defeat:
                messages.SetMessage("You were defeated!", ExitCombat);
                break;
            case BattleOutcome.Run:
                messages.SetMessage("You ran away!", ExitCombat);
                break;
        }
    }
    
    private void OnCombatantTakeDamage(Combatant combatant, int damage)
    {
        Debug.Log($"{combatant.Name} took {damage} damage. " + $"HP: {combatant.CurrentHealth}/{combatant.MaxHealth}");
    }

    private void OnCombatantHeal(Combatant combatant, int amount)
    {
        Debug.Log($"{combatant.Name} healed {amount} HP. " + $"HP: {combatant.CurrentHealth}/{combatant.MaxHealth}");
    }

    private void OnCombatantFainted(Combatant combatant)
    {
        Debug.Log($"{combatant.Name} fainted.");
    }
}