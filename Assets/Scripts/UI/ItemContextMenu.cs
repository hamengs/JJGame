using UnityEngine;

public class ItemContextMenu : MonoBehaviour
{
    private InventorySlot linkedSlot;

    // ���õ�ǰ�˵������Ĳ�λ
    public void Setup(InventorySlot slot)
    {
        linkedSlot = slot;
    }

    public void OnUseButtonClicked()
    {
        Debug.Log("ʹ����Ʒ: " + linkedSlot.itemData.itemName);
        InventoryManager.Instance.UseItem(linkedSlot.itemData);
        // ���ʹ�ú�ϣ���ر������Ĳ˵�������������������
        Destroy(gameObject);
    }

    public void OnDropButtonClicked()
    {
        Debug.Log("������Ʒ: " + linkedSlot.itemData.itemName);
        InventoryManager.Instance.RemoveItem(linkedSlot.itemData);
        Destroy(gameObject);
    }
}


