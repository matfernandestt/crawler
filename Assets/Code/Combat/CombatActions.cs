using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatActions : MonoBehaviour
{
    [SerializeField] private GameButton fightButton;
    [SerializeField] private GameButton libraryButton;
    [SerializeField] private GameButton runButton;

    private readonly List<GameButton> _allButtons = new();

    public Action<BattleActionType> OnActionSelected;

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
        OnActionSelected?.Invoke(BattleActionType.Skill);
    }

    private void Library()
    {
        OnActionSelected?.Invoke(BattleActionType.Item);
    }

    private void Run()
    {
        OnActionSelected?.Invoke(BattleActionType.Run);
    }

    public void SetAllButtonsInteractability(bool interactable)
    {
        foreach (var button in _allButtons)
        {
            button.SetInteractable(interactable);
        }
    }
}