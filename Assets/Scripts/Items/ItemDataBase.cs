using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> allItems; // �洢��Ϸ��������Ʒ����

    // �������Ʋ�����Ʒ
    public Item GetItemByName(string name)
    {
        foreach (Item item in allItems)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }

    // ����ID������Ʒ
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
