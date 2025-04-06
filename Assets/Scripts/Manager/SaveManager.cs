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
    /// ������Ϸ���ݵ� PlayerPrefs��Ҳ���Ա��浽�ļ��У�
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
        // �����������
        data.health = PlayerManager.Instance.playerData.health;
        data.money = PlayerManager.Instance.playerData.money;
        data.attackPower = PlayerManager.Instance.playerData.attackPower;

        // ����������
        data.inventoryItemNames = new List<string>();
        data.inventoryItemCounts = new List<int>();
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        foreach (Item item in InventoryManager.Instance.inventoryData.items)
        {
            Debug.Log("��������Ʒ��" + item.itemName);
            if (itemCounts.ContainsKey(item.itemName))
                itemCounts[item.itemName]++;
            else
                itemCounts[item.itemName] = 1;
        }
        Debug.Log("����ʱͳ�Ƶ���Ʒ����������" + itemCounts.Count);

        foreach (var pair in itemCounts)
        {
            data.inventoryItemNames.Add(pair.Key);
            data.inventoryItemCounts.Add(pair.Value);
        }

        // ���渴��㣺��� currentPortal ���ڣ��򱣴����ʶ
        if (SceneController.Instance.currentPortal != null)
        {
            data.savedPortalEnum = (int)SceneController.Instance.currentPortal.transitionDestination;
        }
        else
        {
            data.savedPortalEnum = -1; // -1 ��ʾû�и����
        }

        data.savedScene = SceneManager.GetActiveScene().name;
        savedScene = data.savedScene;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Game saved: " + json);
    }

    /// <summary>
    /// �Ӵ浵��������
    /// </summary>
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("���ص���Ʒ�����б�" + string.Join(", ", data.inventoryItemNames));

            // �ָ�������� ��Ȼ����UIͨ��playermanager������¼�
            PlayerManager.Instance.SetHealth(data.health);
            PlayerManager.Instance.SetMoney(data.money);
            PlayerManager.Instance.SetAttackPower(data.attackPower);


            // �ָ��������
            InventoryManager.Instance.inventoryData.items.Clear();
            // ʹ����Ʒ���ݿ������Ʒ
            for (int i = 0; i < data.inventoryItemNames.Count; i++)
            {
                string itemName = data.inventoryItemNames[i];
                int count = data.inventoryItemCounts[i];

                Item itemDefinition = itemDataBase.GetItemByName(itemName);

                if (itemDefinition != null)
                {
                    // ���ָ����������Ʒ
                    for (int j = 0; j < count; j++)
                    {
                        // ���������Ҫ������Ʒ�ĸ�����ȡ�����������
                        InventoryManager.Instance.inventoryData.items.Add(itemDefinition);
                    }
                    Debug.Log($"�ɹ�������Ʒ��{itemName} x {count}");
                }
                else
                {
                    Debug.LogWarning("δ�������ݿ����ҵ���Ʒ��" + itemName);
                }
            }
            UIManager.Instance.SetInventory(true);

            UIManager.Instance.RefreshInventoryUI();

            // �ָ������
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
                Debug.Log("savedPortal = -1��û�ҵ���");
            }
            //��deadinformation�������Ϊfalse��loadgame֮����Ҫ��ʾdeadinformation
            if (UIManager.Instance.deadInformation != null)
            {
                UIManager.Instance.HideDeathInformation();
            }

            //ͬ���İ����ñ����Ի�,��Ȼ�����ͶԻ�����ʾ����
            if (SceneManager.GetActiveScene().name != "SceneMenu")
            {
                UIManager.Instance.HideInventory();
                UIManager.Instance.HideChat();
            }


            //������ĸ���Ŀ����֮ǰ������Ϊnull���������������û���
            GameManager.Instance.TrackPlayer();
            //��Ϊ��playercontroller��deadth���õ�ʱ��������death״̬����Ҫ�л���idle����Ȼ������޷��ƶ���
            //��Ϊ��playercontroller�����update�ж�����״ֱ̬��return
            PlayerManager.Instance.playerController.playerState = PlayerController.PlayerStates.Idle;

            //��ȡsaveScene
            savedScene = data.savedScene;
            Debug.Log("Game loaded: " + json);
        }
        else
        {
            Debug.LogWarning("û���ҵ��浵����");
        }
    }
}

