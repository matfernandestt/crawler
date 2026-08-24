using UnityEngine;

[CreateAssetMenu(fileName = "SkillAction_EvasionBonus", menuName = "Data/Skills Action/Evasion Bonus")]
public class EvasionBonusSkillAction : BaseSkillAction
{
    [SerializeField] private int bonusEvasion;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        PlayerReferences.Instance.attributes.AddBonusEvasion(bonusEvasion);
    }
}
