using System;
using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    
    public CombatMessages messages;
    
    public bool InCombat { get; private set; }
    public EnemyData CurrentEnemy { get; private set; }
    
    [SerializeField] private GameObject combatButtons;
    [SerializeField] private EnemyDatabase database;
    [SerializeField] private EnemyVisual enemyVisual;
    [SerializeField] private CombatActions actions;
    [SerializeField] private SkillWindow skillWindow;
    
    private CombatActors roundOwnerActor;
    private Coroutine roundRoutine;

    private void Awake()
    {
        Instance = this;
        
        combatButtons.SetActive(false);
        enemyVisual.gameObject.SetActive(false);
        skillWindow.gameObject.SetActive(false);
        
        actions.OnRunAction += OnRunAction;
    }

    private void OnDestroy()
    {
        actions.OnRunAction -= OnRunAction;
    }

    public void EnterCombat()
    {
        InCombat = true;
        PlayerReferences.Instance.input.SetBlockMovement(true);
        TransitionManager.Instance.Fade(OnFadedToCombat, OnCompletedFadeToCombat);
        actions.SetAllButtonsInteractability(false);
    }

    public void ExitCombat()
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
        CurrentEnemy = database.GetRandomEnemy();
        CurrentEnemy.InitializeEnemy();
        enemyVisual.SetupEnemy(CurrentEnemy);
        skillWindow.SetupSkills(PlayerReferences.Instance.attributes.GetData.availableSkills);

        var playerTransform = PlayerReferences.Instance.transform;
        enemyVisual.gameObject.SetActive(true);
        enemyVisual.transform.position = playerTransform.position + (playerTransform.forward * 10f) + Vector3.up * 2f;
        enemyVisual.transform.forward =  playerTransform.forward;
    }

    private void OnCompletedFadeToCombat()
    {
        if(roundRoutine != null)
            StopCoroutine(roundRoutine);
        roundRoutine = StartCoroutine(RoundRoutine());
    }

    private void SuccessfullyExitedCombat()
    {
        PlayerReferences.Instance.input.SetBlockMovement(false);
    }

    private IEnumerator RoundRoutine()
    {
        var confirmedMessage = false;
        messages.SetMessage("An enemy appeared!", () => { confirmedMessage = true; });
        while (!confirmedMessage) yield return null;

        var playerAttributes = PlayerReferences.Instance.attributes;
        yield return new WaitForSeconds(.5f);

        roundOwnerActor = playerAttributes.CurrentAccuracy >= CurrentEnemy.accuracy ? CombatActors.Player : CombatActors.Enemy;
        
        while (playerAttributes.CurrentHealth > 0 || CurrentEnemy.health > 0)
        {
            var waitingForActionConclusion = false;
            switch (roundOwnerActor)
            {
                case CombatActors.Player:
                    var usedAction = EncounterActions.None;
                    actions.SetAllButtonsInteractability(true);
                    actions.OnCompleteAction += (EncounterActions actionPressed) =>
                    {
                        usedAction = actionPressed;
                        waitingForActionConclusion = true;
                        actions.OnCompleteAction = null;
                    };
                    while (!waitingForActionConclusion) yield return null;
                    waitingForActionConclusion = false;
                    switch (usedAction)
                    {
                        case EncounterActions.Fight:
                            actions.SetAllButtonsInteractability(false);
                            skillWindow.gameObject.SetActive(true);
                            SkillData skill = null;
                            skillWindow.OnPressSkill = usedSkill =>
                            {
                                skill = usedSkill;
                                skillWindow.gameObject.SetActive(false);
                                waitingForActionConclusion = true;
                            };
                            while (!waitingForActionConclusion) yield return null;
                            waitingForActionConclusion = false;
                            messages.SetMessage($"Used {skill.skillName}!", () =>
                            {
                                PlayerReferences.Instance.animations.SetAttack();
                                waitingForActionConclusion = true;
                            });
                            while (!waitingForActionConclusion) yield return null;
                            yield return new WaitForSeconds(1f);
                            actions.SetAllButtonsInteractability(true);
                            skill.ProcessSkill();
                            CurrentEnemy.OnDeath = () => { StartCoroutine(EnemyDeathRoutine()); };
                            CurrentEnemy.TakeDamage(skill.power);
                            roundOwnerActor = CombatActors.Enemy;
                            break;
                        case EncounterActions.Library:
                            break;
                    }

                    break;
                case CombatActors.Enemy:
                    actions.SetAllButtonsInteractability(false);
                    yield return new WaitForSeconds(.5f);
                    playerAttributes.TakeDamage(CurrentEnemy.attack);
                    PlayerReferences.Instance.animations.SetTakeDamage();
                    if(playerAttributes.CurrentHealth <= 0)
                        PlayerDeath();
                    yield return new WaitForSeconds(1f);
                    roundOwnerActor = CombatActors.Player;
                    break;
            }
        }
    }

    private IEnumerator EnemyDeathRoutine()
    {
        skillWindow.gameObject.SetActive(false);
        if(roundRoutine != null)
            StopCoroutine(roundRoutine);
        actions.SetAllButtonsInteractability(false);
        yield return new WaitForSeconds(.5f);
        enemyVisual.gameObject.SetActive(false);
        messages.SetMessage("Enemy defeated!", ExitCombat);
    }

    private void PlayerDeath()
    {
        skillWindow.gameObject.SetActive(false);
        actions.SetAllButtonsInteractability(false);
        if(roundRoutine != null)
            StopCoroutine(roundRoutine);
    }
    
    private void OnRunAction()
    {
        if(roundRoutine != null)
            StopCoroutine(roundRoutine);
    }
}

public enum CombatActors
{
    Player,
    Enemy
}