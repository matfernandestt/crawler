using UnityEngine;

public class SkillData : ScriptableObject
{
    public string skillName;
    public int power;
    public int accuracy;
    public int cost;
    public BaseSkillAction action;

    public void ProcessSkill()
    {
        if (action == null) return;
        action.ExecuteAction();
    }
}