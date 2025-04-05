using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> allItems; // 存储游戏中所有物品定义

    // 根据名称查找物品
    public Item GetItemByName(string name)
    {
        foreach (Item item in allItems)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }

    // 根据ID查找物品
    public Item GetItemById(string id)
    {
        foreach (Item item in allItems)
        {
            if (item.id == id)
                return item;
        }
        return null;
    }
}
