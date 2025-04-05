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
        Debug.Log("使用物品: " + linkedSlot.itemData.itemName);
        InventoryManager.Instance.UseItem(linkedSlot.itemData);
        // 如果使用后希望关闭上下文菜单，可以在这里销毁它
        Destroy(gameObject);
    }

    public void OnDropButtonClicked()
    {
        Debug.Log("丢弃物品: " + linkedSlot.itemData.itemName);
        InventoryManager.Instance.RemoveItem(linkedSlot.itemData);
        Destroy(gameObject);
    }
}


