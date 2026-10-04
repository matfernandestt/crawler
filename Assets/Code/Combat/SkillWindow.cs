using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillWindow : MonoBehaviour
{
    [SerializeField] private GameButton buttonPrefab;
    [SerializeField] private GameButton backButton;
    [SerializeField] private Transform objParent;

    public Action<SkillData> OnSkillSelected;
    public Action onBack;

    private InputMap _input;

    private void Awake()
    {
        _input = new InputMap();
        backButton.Button.onClick.AddListener(Back);
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.CloseWindow.started += BackInput;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.CloseWindow.started -= BackInput;
    }
    
    private void BackInput(InputAction.CallbackContext obj)
    {
        Back();
    }

    private void Back()
    {
        onBack?.Invoke();
    }

    private void ClearButtons()
    {
        foreach (Transform child in objParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetupSkills(SkillData[] skills)
    {
        ClearButtons();

        foreach (var skill in skills)
        {
            var newButton = Instantiate(buttonPrefab, objParent);

            newButton.SetText(skill.skillName);

            newButton.Button.onClick.AddListener(() =>
            {
                OnSkillSelected?.Invoke(skill);
            });
        }
    }
}