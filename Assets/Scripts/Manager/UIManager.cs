using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public Shop shop;
    public InventoryData inventory;

    [Header("UI References")]
    [SerializeField] private Canvas canvas;           // 直接拖拽 Canvas
    [SerializeField] public GameObject canvasObject;   
    [SerializeField] private Transform inventoryPanel;  // 背包面板
    [SerializeField] private GameObject inventorySlotPrefab;

    public TextMeshProUGUI health;
    public TextMeshProUGUI money;
    public TextMeshProUGUI attack;
    public GameObject inventoryGameObject;
    public Button closeButton;
    public GameObject deadInformation;
    public Button restartButton;
    public Button exitButton;



    private void Awake()
    {
        base.Awake();


    }

    private void Start()
    {
        // 初始化值
        health.text = "Health: " + PlayerManager.Instance.playerData.health;
        money.text = "Money: " + PlayerManager.Instance.playerData.money;
        attack.text = "Attack: " + PlayerManager.Instance.playerData.attackPower;

        deadInformation.SetActive(false);
        inventory.items = new List<Item>();
        RefreshInventoryUI();

        // 默认关闭背包，隐藏 Canvas
 
            HideInventory();
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && canvas.enabled == false)
        {
            ShowInventory();
        }
        else if (Input.GetKeyDown(KeyCode.B) && canvas.enabled == true)
        {
            HideInventory();
        }
    }

    public void AddItemToInventory(Item item)
    {
        inventory.items.Add(item);
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        Dictionary<Item, int> itemCount = new Dictionary<Item, int>();
        foreach (Item item in inventory.items)
        {
            if (itemCount.ContainsKey(item))
                itemCount[item]++;
            else
                itemCount[item] = 1;
        }

        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in itemCount)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, inventoryPanel);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            slot.SetSlot(item.Key, item.Value);
        }
    }

    public void ShowInventory()
    {
        if (canvasObject != null)
        {
            canvasObject.GetComponent<Canvas>().enabled = true;
            RefreshInventoryUI();
        }
    }

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

    public void OnStartButtonClicked()
    {
        SceneManager.LoadSceneAsync("Scene1");
    }

    public void OnContinueButtonClicked()
    {
        // 继续游戏逻辑
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }


    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene("Scene1");
    }


    public void GameOver()
    {
        Debug.Log("Gameover");
        if (deadInformation != null)
        {
            canvas.enabled = true;
            inventoryGameObject.SetActive(false);
            deadInformation.SetActive(true);
        }

    }
}
