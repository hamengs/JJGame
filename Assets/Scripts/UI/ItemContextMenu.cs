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
        // ������Ʒ��ʹ�÷�������֪ͨ
        Debug.Log("ʹ����Ʒ: " + linkedSlot.itemData.itemName);
        if (linkedSlot.itemData.itemType == Item.ItemType.Potion)
        {
            //ͨ�������������manager����Ѫ��
            PlayerManager.Instance.PlusHealth(linkedSlot.itemData.healthPlus);
        }
        if (linkedSlot.itemData.itemType == Item.ItemType.Food)
        {
            //ͨ�������������manager����Ѫ�����߹�����
            PlayerManager.Instance.PlusHealth(linkedSlot.itemData.healthPlus);
            PlayerManager.Instance.PlusAttackPower(linkedSlot.itemData.attackPlus);
        }
        UIManager.Instance.inventory.items.Remove(linkedSlot.itemData);
        UIManager.Instance.RefreshInventoryUI();

    }

    public void OnDropButtonClicked()
    {
        Debug.Log("������Ʒ: " + linkedSlot.itemData.itemName);
        // ������Ҫִ�ж����߼�������� InventoryData ���Ƴ�����ˢ�� UI
        UIManager.Instance.inventory.items.Remove(linkedSlot.itemData);
        UIManager.Instance.RefreshInventoryUI();
    }
}

