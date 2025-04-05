using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<Item> items;

    /// <summary>
    /// ������Ʒ���Ʋ��Ҷ�Ӧ����Ʒ
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
    /// ������Ʒ��ΨһID������Ʒ��������ж���ID��
    /// </summary>
    public Item GetItemByID(string id)
    {
        foreach (Item item in items)
        {
            // ���� Item ������һ�� id �ֶ�
            if (item.id == id)
                return item;
        }
        return null;
    }
}
