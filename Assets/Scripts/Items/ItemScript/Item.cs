using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemSO")]
public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite icon;
    public ItemType itemType;
    public GameObject GameObjectItem;
    public string description;
    public int healthPlus;
    public int attackPlus;

    public enum ItemType
    {
        Potion, Weapon, Food
    }



}
