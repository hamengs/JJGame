using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private const string SaveKey = "GameSaveData";
    public InventoryData inventoryData;
    public ItemDatabase itemDataBase;
    public string savedScene;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        inventoryData = InventoryManager.Instance.inventoryData;
    }

    /// <summary>
    /// 保存游戏数据到 PlayerPrefs（也可以保存到文件中）
    /// </summary>
    /// 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("SceneMenu");
        }
    }
    public void SaveGame()
    {
        SaveData data = new SaveData();
        // 保存玩家数据
        data.health = PlayerManager.Instance.playerData.health;
        data.money = PlayerManager.Instance.playerData.money;
        data.attackPower = PlayerManager.Instance.playerData.attackPower;

        // 保存库存数据
        data.inventoryItemNames = new List<string>();
        data.inventoryItemCounts = new List<int>();
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        foreach (Item item in InventoryManager.Instance.inventoryData.items)
        {
            Debug.Log("保存库存物品：" + item.itemName);
            if (itemCounts.ContainsKey(item.itemName))
                itemCounts[item.itemName]++;
            else
                itemCounts[item.itemName] = 1;
        }
        Debug.Log("保存时统计到物品种类数量：" + itemCounts.Count);

        foreach (var pair in itemCounts)
        {
            data.inventoryItemNames.Add(pair.Key);
            data.inventoryItemCounts.Add(pair.Value);
        }

        // 保存复活点：如果 currentPortal 存在，则保存其标识
        if (SceneController.Instance.currentPortal != null)
        {
            data.savedPortalEnum = (int)SceneController.Instance.currentPortal.transitionDestination;
        }
        else
        {
            data.savedPortalEnum = -1; // -1 表示没有复活点
        }

        data.savedScene = SceneManager.GetActiveScene().name;
        savedScene = data.savedScene;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Game saved: " + json);
    }

    /// <summary>
    /// 从存档加载数据
    /// </summary>
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("加载的物品名称列表：" + string.Join(", ", data.inventoryItemNames));

            // 恢复玩家数据 自然更新UI通过playermanager里面的事件
            PlayerManager.Instance.SetHealth(data.health);
            PlayerManager.Instance.SetMoney(data.money);
            PlayerManager.Instance.SetAttackPower(data.attackPower);


            // 恢复库存数据
            InventoryManager.Instance.inventoryData.items.Clear();
            // 使用物品数据库加载物品
            for (int i = 0; i < data.inventoryItemNames.Count; i++)
            {
                string itemName = data.inventoryItemNames[i];
                int count = data.inventoryItemCounts[i];

                Item itemDefinition = itemDataBase.GetItemByName(itemName);

                if (itemDefinition != null)
                {
                    // 添加指定数量的物品
                    for (int j = 0; j < count; j++)
                    {
                        // 这里可能需要创建物品的副本，取决于您的设计
                        InventoryManager.Instance.inventoryData.items.Add(itemDefinition);
                    }
                    Debug.Log($"成功加载物品：{itemName} x {count}");
                }
                else
                {
                    Debug.LogWarning("未能在数据库中找到物品：" + itemName);
                }
            }
            UIManager.Instance.SetInventory(true);

            UIManager.Instance.RefreshInventoryUI();

            // 恢复复活点
            if (data.savedPortalEnum != -1)
            {
                TransitionTo savedPortal = (TransitionTo)data.savedPortalEnum;
                if (SceneController.Instance.portalMapping.TryGetValue(savedPortal, out TransitionPoint portal))
                {
                    PlayerManager.Instance.playerController.gameObject.SetActive(true);
                    SceneController.Instance.currentPortal = portal;
                    if (portal != null)
                    {
                        PlayerManager.Instance.playerController.transform.position = portal.transform.Find("Destination").transform.position;
                    }
                    
                }
            }
            else
            {
                Debug.Log("savedPortal = -1，没找到！");
            }
            //把deadinformation组件设置为false，loadgame之后不需要显示deadinformation
            if (UIManager.Instance.deadInformation != null)
            {
                UIManager.Instance.HideDeathInformation();
            }

            //同样的把设置背包对话,不然背包和对话会显示出来
            if (SceneManager.GetActiveScene().name != "SceneMenu")
            {
                UIManager.Instance.HideInventory();
                UIManager.Instance.HideChat();
            }


            //摄像机的跟踪目标在之前被设置为null，在这里重新设置回来
            GameManager.Instance.TrackPlayer();
            //因为在playercontroller里deadth调用的时候人物是death状态，需要切换回idle，不然会造成无法移动，
            //因为在playercontroller里面的update判断死亡状态直接return
            PlayerManager.Instance.playerController.playerState = PlayerController.PlayerStates.Idle;

            //读取saveScene
            savedScene = data.savedScene;
            Debug.Log("Game loaded: " + json);
        }
        else
        {
            Debug.LogWarning("没有找到存档数据");
        }
    }
}

