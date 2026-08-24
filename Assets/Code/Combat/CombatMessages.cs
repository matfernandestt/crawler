using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatMessages : MonoBehaviour
{
    [SerializeField] private GameObject messageBox;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private GameObject waitingConfirmation;

    private InputMap _input;
    private Action onConfirm;
    private bool _activeMessage;

    private void Awake()
    {
        _input = new InputMap();
        _input.Enable();
        
        _input.Player.Click.started += OnConfirmMessage;
        
        messageBox.SetActive(false);
    }

    private void OnConfirmMessage(InputAction.CallbackContext obj)
    {
        if (!_activeMessage) return;
        onConfirm?.Invoke();
        messageBox.SetActive(false);
        _activeMessage = false;
    }

    public void SetMessage(string message, Action confirmMessageAction)
    {
        messageBox.SetActive(true);
        StartCoroutine(AddMessageCharacters(message));
        onConfirm = confirmMessageAction;
    }

    private IEnumerator AddMessageCharacters(string message)
    {
        waitingConfirmation.SetActive(false);
        textField.text = "";
        foreach (var c in message)
        {
            textField.text += c;
            yield return null;
            yield return null;
        }

        yield return null;
        waitingConfirmation.SetActive(true);
        _activeMessage = true;
    }
}