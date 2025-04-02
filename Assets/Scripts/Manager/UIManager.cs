using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : Singleton<UIManager>
{
    public Shop shop;
    public InventoryData inventory;


    public GameObject canvasObject;
    private Canvas canvas;

    // InventoryPanel ���󣬼�������в�λ������
    public Transform inventoryPanel;
    // Ԥ�Ƶ���Ʒ��λ��InventorySlot��Ԥ�Ƽ�
    public GameObject inventorySlotPrefab;


    public TextMeshProUGUI health;
    public TextMeshProUGUI money;
    public TextMeshProUGUI attack;

    private void Awake()
    {
        base.Awake();
        canvasObject = this.gameObject;
        canvas = GetComponent<Canvas>();
}

    private void Start()
    {

        //��ʼ��value
        health.text = "Health: " + PlayerManager.Instance.playerData.health;
        money.text = "Money: " + PlayerManager.Instance.playerData.money;
        attack.text = "Attack: " + PlayerManager.Instance.playerData.attackPower;


        inventory.items = new List<Item>();
        RefreshInventoryUI();

        // ��¼ Canvas ��ԭʼλ��
        if (canvasObject != null)
        {
            canvasObject.GetComponent<Canvas>().enabled = false;
        }
        // Ĭ�Ϲرձ������ƶ� Canvas ������λ��
        HideInventory();
        RefreshInventoryUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)&&canvas.enabled==false)
        {
            ShowInventory();
        }else if(Input.GetKeyDown(KeyCode.B) && canvas.enabled == true)
        {
            HideInventory();
        }
    }

    public void AddItemToInventory(Item item)
    {
        inventory.items.Add(item);
        RefreshInventoryUI();
    }

    // ˢ�¿����棬�����������в�λ
    public void RefreshInventoryUI()
    {
        // ���ֵ�ͳ����Ʒ������key Ϊ Item��value Ϊ����
        Dictionary<Item, int> itemCount = new Dictionary<Item, int>();
        foreach (Item item in inventory.items)
        {
            if (itemCount.ContainsKey(item))
            {
                itemCount[item]++;
            }
            else
            {
                itemCount[item] = 1;
            }
        }

        // ��� InventoryPanel ��ԭ�е�������
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // ���� InventoryData �е���Ʒ���ݣ�ʵ������Ӧ�Ĳ�λ
        foreach (var item in itemCount)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, inventoryPanel);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            // ����ÿ����Ʒ����Ϊ 1����������������޸Ĵ˴�
            slot.SetSlot(item.Key, item.Value);
        }
    }

    // ��ʾ�������� Canvas �ƻ�ԭ����λ��
    public void ShowInventory()
    {
        if (canvasObject != null)
        {
            canvasObject.GetComponent<Canvas>().enabled = true;
            RefreshInventoryUI();
        }
    }

    // ���ر������� Canvas �ƶ���Ԥ���������λ��
    public void HideInventory()
    {
        if (canvasObject != null)
        {
            canvasObject.GetComponent<Canvas>().enabled = false;
        }
    }

    public void SetHealthValue(int value)
    {
        health.text = "Health: " + value;
    }


    public void SetMoneyValue(int value)
    {
        money.text = "Money: " + value;
    }


    public void SetAttackValue(int value)
    {
        attack.text = "Attack: " + value;
    }
}
