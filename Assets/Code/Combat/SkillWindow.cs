using System;
using UnityEngine;

public class SkillWindow : MonoBehaviour
{
    [SerializeField] private GameButton buttonPrefab;

    public Action<SkillData> OnPressSkill;

    private void ClearButtons()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetupSkills(SkillData[] skills)
    {
        ClearButtons();
        
        foreach (var skill in skills)
        {
            var newBtn = Instantiate(buttonPrefab, transform);
            newBtn.SetText(skill.skillName);
            newBtn.Button.onClick.AddListener(() =>
            {
                OnPressSkill?.Invoke(skill);
            });
        }
    }
}
