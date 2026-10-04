using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public ItemData[] TestItems;
    [SerializeField] private int testItemAmount = 3;

    public Inventory Inventory { get; private set; }

    public static PlayerInventory Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Inventory = new Inventory();
            foreach (var item in TestItems)
            {
                Inventory.Add(item, testItemAmount);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}