using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatActions : MonoBehaviour
{
    [SerializeField] private GameButton fightButton;
    [SerializeField] private GameButton libraryButton;
    [SerializeField] private GameButton runButton;

    private List<GameButton> _allButtons = new();

    private const string CombatMessage_Run = "You tried to run.";

    public Action<EncounterActions> OnCompleteAction;
    public Action OnRunAction;

    private void Awake()
    {
        _allButtons.Add(fightButton);
        _allButtons.Add(libraryButton);
        _allButtons.Add(runButton);
        
        fightButton.Button.onClick.AddListener(Fight);
        libraryButton.Button.onClick.AddListener(Library);
        runButton.Button.onClick.AddListener(Run);
    }

    private void Fight()
    {
        OnCompleteAction?.Invoke(EncounterActions.Fight);
    }

    private void Library()
    {
        OnCompleteAction?.Invoke(EncounterActions.Library);
    }

    private void Run()
    {
        SetAllButtonsInteractability(false);
        CombatManager.Instance.messages.SetMessage(CombatMessage_Run, () => { CombatManager.Instance.ExitCombat(); });
        OnRunAction?.Invoke();
    }

    public void SetAllButtonsInteractability(bool interactable)
    {
        foreach (var button in _allButtons)
        {
            button.SetInteractable(interactable);
        }
    }
}

public enum EncounterActions
{
    None,
    Fight,
    Library,
    Run
}