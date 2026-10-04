using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemTarget target;
    public BaseItemAction action;
}