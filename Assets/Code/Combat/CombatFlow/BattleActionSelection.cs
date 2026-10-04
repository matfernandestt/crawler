using System;

public class BattleActionSelection
{
    public BattleAction SelectedAction { get; private set; }

    public bool HasSelectedAction => SelectedAction != null;

    public void Select(BattleAction action)
    {
        SelectedAction = action;
    }

    public void Clear()
    {
        SelectedAction = null;
    }
}