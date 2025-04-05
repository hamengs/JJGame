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
    [SerializeField] public Transform inventoryPanel;  // �������
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
        // Ĭ�Ϲرձ���
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
                Debug.LogWarning("δ�ҵ�canvas");
            }
        }
    }



    public void RefreshInventoryUI()
    {
        // ʹ�� InventoryManager �е����ݣ�
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
            // ��� InventoryPanel �е����в�λ
            foreach (Transform child in inventoryPanel)
            {
                Destroy(child.gameObject);
            }

            // ����ͳ�ƽ��ʵ�������߲�λ
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
        inventoryGroup.alpha = 1f;            // ��͸��
        inventoryGroup.interactable = true;    // �ɽ���
        inventoryGroup.blocksRaycasts = true;  // �赲����
    }

    public void HideInventory()
    {
        inventoryGroup.alpha = 0f;            // ��ȫ͸��
        inventoryGroup.interactable = false;    // ���ɽ���
        inventoryGroup.blocksRaycasts = false;  // ���赲����
    }

    public void HideDeathInformation()
    {
        if (deadInformationGroup != null)
        {
            deadInformationGroup.alpha = 0f;            // ��ȫ͸��
            deadInformationGroup.interactable = false;    // ���ɽ���
            deadInformationGroup.blocksRaycasts = false;  // ���赲����
        }

    }

    public void ShowDeathInformation()
    {
        if (deadInformationGroup != null)
        {
            deadInformationGroup.alpha = 1f;            // ͸��
            deadInformationGroup.interactable = true;    // �ɽ���
            deadInformationGroup.blocksRaycasts = true;  // �赲����
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
            Debug.Log("δ�ҵ��浵");
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


    // �ص������������ʱ����
    private void OnPlayerDeathHandler()
    {

        GameOver();

    }

}
