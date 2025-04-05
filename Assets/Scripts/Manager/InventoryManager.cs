using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    // �洢��������е��ߵ��б�
    public InventoryData inventoryData;
    protected override void Awake()
    {
        base.Awake();
        //inventoryData = ScriptableObject.Instantiate(inventoryData);
    }


    /// <summary>
    /// ��ӵ��ߵ���棬��֪ͨ UI ˢ��
    /// </summary>
    public void AddItem(Item item)
    {
        inventoryData.items.Add(item);
        // ֪ͨ UI ˢ�¿����ʾ
        UIManager.Instance.RefreshInventoryUI();
    }

    /// <summary>
    /// �ӿ�����Ƴ����ߣ���֪ͨ UI ˢ��
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
    /// ʹ�õ��ߣ����ݵ�������ִ�в�ͬ������Ȼ��ӿ�����Ƴ�
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
        // �����Ҫ�������͵ĵ����߼����������
        RemoveItem(item);
    }
}

