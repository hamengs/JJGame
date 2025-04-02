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

    // InventoryPanel 对象，即存放所有槽位的容器
    public Transform inventoryPanel;
    // 预制的物品槽位（InventorySlot）预制件
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

        //初始化value
        health.text = "Health: " + PlayerManager.Instance.playerData.health;
        money.text = "Money: " + PlayerManager.Instance.playerData.money;
        attack.text = "Attack: " + PlayerManager.Instance.playerData.attackPower;


        inventory.items = new List<Item>();
        RefreshInventoryUI();

        // 记录 Canvas 的原始位置
        if (canvasObject != null)
        {
            canvasObject.GetComponent<Canvas>().enabled = false;
        }
        // 默认关闭背包，移动 Canvas 到隐藏位置
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

    // 刷新库存界面，重新生成所有槽位
    public void RefreshInventoryUI()
    {
        // 用字典统计物品数量：key 为 Item，value 为数量
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

        // 清空 InventoryPanel 中原有的子物体
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // 遍历 InventoryData 中的物品数据，实例化对应的槽位
        foreach (var item in itemCount)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, inventoryPanel);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            // 假设每个物品数量为 1，如有数量管理可修改此处
            slot.SetSlot(item.Key, item.Value);
        }
    }

    // 显示背包：将 Canvas 移回原来的位置
    public void ShowInventory()
    {
        if (canvasObject != null)
        {
            canvasObject.GetComponent<Canvas>().enabled = true;
            RefreshInventoryUI();
        }
    }

    // 隐藏背包：将 Canvas 移动到预定义的隐藏位置
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
