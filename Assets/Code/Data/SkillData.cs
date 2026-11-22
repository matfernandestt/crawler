using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillDamageType damageType;
    public int power;
    public int accuracy;
    public int cost;
    public int priority;
    public BaseSkillAction action;
    public SkillEffectType effectType;
}