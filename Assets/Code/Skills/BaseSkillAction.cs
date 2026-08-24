using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillAction", menuName = "Data/SkillAction")]
public class BaseSkillAction: ScriptableObject
{
    public virtual void ExecuteAction() { }
}