using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<Item> items;

    /// <summary>
    /// 根据物品名称查找对应的物品
    /// </summary>
    public Item GetItemByName(string name)
    {
        foreach (Item item in items)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }

    /// <summary>
    /// 根据物品的唯一ID查找物品（如果你有定义ID）
    /// </summary>
    public Item GetItemByID(string id)
    {
        foreach (Item item in items)
        {
            // 假设 Item 类中有一个 id 字段
            if (item.id == id)
                return item;
        }
        return null;
    }
}
