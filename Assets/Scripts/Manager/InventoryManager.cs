using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    // 存储库存中所有道具的列表
    public InventoryData inventoryData;
    protected override void Awake()
    {
        base.Awake();
        //inventoryData = ScriptableObject.Instantiate(inventoryData);
    }


    /// <summary>
    /// 添加道具到库存，并通知 UI 刷新
    /// </summary>
    public void AddItem(Item item)
    {
        inventoryData.items.Add(item);
        // 通知 UI 刷新库存显示
        UIManager.Instance.RefreshInventoryUI();
    }

    /// <summary>
    /// 从库存中移除道具，并通知 UI 刷新
    /// </summary>
    public void RemoveItem(Item item)
    {
        if (inventoryData.items.Contains(item))
        {
            inventoryData.items.Remove(item);
            UIManager.Instance.RefreshInventoryUI();
        }
    }

    /// <summary>
    /// 使用道具，根据道具类型执行不同操作，然后从库存中移除
    /// </summary>
    public void UseItem(Item item)
    {
        if (item.itemType == Item.ItemType.Potion)
        {
            PlayerManager.Instance.PlusHealth(item.healthPlus);
        }
        else if (item.itemType == Item.ItemType.Food)
        {
            PlayerManager.Instance.PlusHealth(item.healthPlus);
            PlayerManager.Instance.PlusAttackPower(item.attackPlus);
        }
        // 如果需要其他类型的道具逻辑，继续添加
        RemoveItem(item);
    }
}

