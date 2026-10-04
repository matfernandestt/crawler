using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillAction", menuName = "Data/SkillAction")]
public abstract class BaseSkillAction : ScriptableObject
{
    public abstract void Execute(BattleContext context, Combatant user, Combatant target, SkillData skill);
}