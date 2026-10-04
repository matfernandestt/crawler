using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemWindow : MonoBehaviour
{
    [SerializeField] private GameButton buttonPrefab;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameButton backButton;

    public Action<ItemData> OnItemSelected;
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
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetupItems(Inventory inventory)
    {
        ClearButtons();

        foreach (var item in inventory.Items)
        {
            var amount = inventory.GetAmount(item);

            if (amount <= 0)
                continue;

            var newButton = Instantiate(buttonPrefab, scrollViewContent);

            newButton.SetText($"{item.itemName} x{amount}");
            newButton.SetImage(item.itemIcon);

            newButton.Button.onClick.AddListener(() =>
            {
                OnItemSelected?.Invoke(item);
            });
        }
    }
}

public enum ItemTarget
{
    Self,
    Opponent
}