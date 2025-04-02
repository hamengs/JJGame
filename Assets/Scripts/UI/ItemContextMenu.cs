using UnityEngine;

public class ItemContextMenu : MonoBehaviour
{
    private InventorySlot linkedSlot;

    // 设置当前菜单关联的槽位
    public void Setup(InventorySlot slot)
    {
        linkedSlot = slot;
    }

    public void OnUseButtonClicked()
    {
        // 调用物品的使用方法或发送通知
        Debug.Log("使用物品: " + linkedSlot.itemData.itemName);
        if (linkedSlot.itemData.itemType == Item.ItemType.Potion)
        {
            //通过调用玩家数据manager来加血量
            PlayerManager.Instance.PlusHealth(linkedSlot.itemData.healthPlus);
        }
        if (linkedSlot.itemData.itemType == Item.ItemType.Food)
        {
            //通过调用玩家数据manager来加血量或者攻击力
            PlayerManager.Instance.PlusHealth(linkedSlot.itemData.healthPlus);
            PlayerManager.Instance.PlusAttackPower(linkedSlot.itemData.attackPlus);
        }
        UIManager.Instance.inventory.items.Remove(linkedSlot.itemData);
        UIManager.Instance.RefreshInventoryUI();

    }

    public void OnDropButtonClicked()
    {
        Debug.Log("丢弃物品: " + linkedSlot.itemData.itemName);
        // 根据需要执行丢弃逻辑，例如从 InventoryData 中移除，并刷新 UI
        UIManager.Instance.inventory.items.Remove(linkedSlot.itemData);
        UIManager.Instance.RefreshInventoryUI();
    }
}

