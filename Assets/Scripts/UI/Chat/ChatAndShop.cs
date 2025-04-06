using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatAndShop : MonoBehaviour
{
    public GameObject chatAndShopObj;
    public CanvasGroup chatAndShopCanvasGroup;
    public Button closeButton;
    public Button nextButton;
    public Button shopButton;
    public TextMeshProUGUI nextButtonText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI npcNameText;
    public NPCData npcData;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chatAndShopObj = this.gameObject;

        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        nextButton = transform.Find("NextButton").GetComponent<Button>();
        shopButton = transform.Find("ShopButton").GetComponent<Button>();

        nextButtonText = nextButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText = transform.Find("ChatBackGround/ChatText").GetComponent<TextMeshProUGUI>();
        npcNameText = transform.Find("NPCNameBackGround/NPCNameText").GetComponent<TextMeshProUGUI>();

        chatAndShopCanvasGroup = GetComponent<CanvasGroup>();

        UIManager.Instance.chatAndShop = this;
        UIManager.Instance.chatAndShopGroup = this.chatAndShopCanvasGroup;

        closeButton.onClick.AddListener(UIManager.Instance.HideChat);
        nextButton.onClick.AddListener(UIManager.Instance.OnChatNextButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
