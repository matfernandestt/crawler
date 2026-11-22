using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private GameButton buttonPrefab;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject window;
    
    private Action<ItemData> OnItemSelected;
    
    private InputMap _input;

    private void Awake()
    {
        _input = new InputMap();
        window.SetActive(false);
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.CloseWindow.started += ToggleWindow;
        OnItemSelected += ItemSelected;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.CloseWindow.started -= ToggleWindow;
        OnItemSelected -= ItemSelected;
    }
    
    private void ToggleWindow(InputAction.CallbackContext obj)
    {
        if (window.activeSelf)
        {
            window.SetActive(false);
        }
        else
        {
            SetupItems(PlayerInventory.Instance.Inventory);
            window.SetActive(true);
        }
    }

    private void ClearButtons()
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetupItems(Inventory inventory)
    {
        ClearButtons();

        foreach (var item in inventory.Items)
        {
            if(item.target == ItemTarget.Opponent) continue;
            
            var amount = inventory.GetAmount(item);

            if (amount <= 0)
                continue;

            for (var i = 0; i < amount; i++)
            {
                var newButton = Instantiate(buttonPrefab, scrollViewContent);

                newButton.SetText($"{item.itemName}");
                newButton.SetImage(item.itemIcon);

                newButton.Button.onClick.AddListener(() =>
                {
                    OnItemSelected?.Invoke(item);
                });
            }
        }
    }

    private void ItemSelected(ItemData item)
    {
        var inventory = PlayerInventory.Instance.Inventory;
        inventory.TryUse(item);
        
        item.action.OverworldExecute(item);
        SetupItems(inventory);
        window.SetActive(false);
    }
}