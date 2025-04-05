using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    public Shop shop;
    public InventoryData inventory;

    [Header("UI References")]
    [SerializeField] public Canvas canvas;           
    [SerializeField] public GameObject canvasObject;   
    [SerializeField] public Transform inventoryPanel;  // 背包面板
    [SerializeField] public GameObject inventorySlotPrefab;

    public GameObject inventoryGameObject;
    public Button closeButton;
    public GameObject deadInformation;
    public Button restartButton;
    public Button exitButton;

    public CanvasGroup inventoryGroup;
    public CanvasGroup deadInformationGroup;




    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void Start()
    {

        RefreshInventoryUI();
        // 默认关闭背包
        if (SceneManager.GetActiveScene().name!="SceneMenu")
        {
            HideInventory();
        }
        HideDeathInformation();

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnPlayerDeath += OnPlayerDeathHandler;
        }
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnPlayerDeath -= OnPlayerDeathHandler;
        }
        base.OnDestroy();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)&&inventoryGroup.alpha==0)
        {
            ShowInventory();
        }
        else if (Input.GetKeyDown(KeyCode.B) && inventoryGroup.alpha == 1)
        {
            HideInventory();
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (canvasObject == null)
        {
            canvasObject = GameObject.FindGameObjectWithTag("Canvas");
            if (canvasObject != null)
            {
                canvas = canvasObject.GetComponent<Canvas>();
            }
            else
            {
                Debug.LogWarning("未找到canvas");
            }
        }
    }



    public void RefreshInventoryUI()
    {
        // 使用 InventoryManager 中的数据，
        Dictionary<Item, int> itemCount = new Dictionary<Item, int>();
        foreach (Item item in InventoryManager.Instance.inventoryData.items)
        {
            if (itemCount.ContainsKey(item))
                itemCount[item]++;
            else
                itemCount[item] = 1;
        }
        if (inventoryPanel != null)
        {
            // 清空 InventoryPanel 中的现有槽位
            foreach (Transform child in inventoryPanel)
            {
                Destroy(child.gameObject);
            }

            // 根据统计结果实例化道具槽位
            foreach (var pair in itemCount)
            {
                GameObject slotGO = Instantiate(inventorySlotPrefab, inventoryPanel);
                InventorySlot slot = slotGO.GetComponent<InventorySlot>();
                slot.SetSlot(pair.Key, pair.Value);
            }
        }

    }


    public void ShowInventory()
    {
        inventoryGroup.alpha = 1f;            // 不透明
        inventoryGroup.interactable = true;    // 可交互
        inventoryGroup.blocksRaycasts = true;  // 阻挡射线
    }

    public void HideInventory()
    {
        inventoryGroup.alpha = 0f;            // 完全透明
        inventoryGroup.interactable = false;    // 不可交互
        inventoryGroup.blocksRaycasts = false;  // 不阻挡射线
    }

    public void HideDeathInformation()
    {
        if (deadInformationGroup != null)
        {
            deadInformationGroup.alpha = 0f;            // 完全透明
            deadInformationGroup.interactable = false;    // 不可交互
            deadInformationGroup.blocksRaycasts = false;  // 不阻挡射线
        }

    }

    public void ShowDeathInformation()
    {
        if (deadInformationGroup != null)
        {
            deadInformationGroup.alpha = 1f;            // 透明
            deadInformationGroup.interactable = true;    // 可交互
            deadInformationGroup.blocksRaycasts = true;  // 阻挡射线
        }

    }


    public void OnStartButtonClicked()
    {
        GameManager.Instance.LoadSceneAndStart("Scene1");
    }

    public void OnContinueButtonClicked()
    {
        if (SaveManager.Instance.savedScene != null)
        {
            GameManager.Instance.LoadSceneAndContinue(SaveManager.Instance.savedScene);
        }
        else
        {
            Debug.Log("未找到存档");
        }
        
    }

    public void OnExitButtonClicked()
    {
        SaveManager.Instance.SaveGame();
        Application.Quit();
    }


    public void OnRestartButtonClicked()
    {
        SaveManager.Instance.LoadGame(); ;
    }


    public void GameOver()
    {
        if (deadInformation != null)
        {
            ShowDeathInformation();
        }

    }

    public void SetCanvas(bool isEnabled)
    {
        if (isEnabled == true)
        {
            canvas.enabled = true;
        }else if(isEnabled == false)
        {
            canvas.enabled = false;
        }
    }

    public void SetInventory(bool isEnabled)
    {
        if (inventoryGameObject != null)
        {
            inventoryGameObject.SetActive(isEnabled);
        }

    }


    // 回调，当玩家死亡时调用
    private void OnPlayerDeathHandler()
    {

        GameOver();

    }

}
