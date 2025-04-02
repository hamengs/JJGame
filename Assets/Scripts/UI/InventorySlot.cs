using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image iconImage;
    public TextMeshProUGUI quantityText;

    public Item itemData;
    public int quantity;

    public GameObject contextMenuPrefab;
    private GameObject currentContextMenu;

    private void Awake()
    {

        iconImage = GetComponentInChildren<Image>();
        quantityText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSlot(Item item, int qty)
    {
        itemData = item;
        quantity = qty;
        iconImage.sprite = item.icon;
        quantityText.text = qty.ToString();

    }
    // ������ʱ��ʾ�����Ĳ˵�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (contextMenuPrefab != null && currentContextMenu == null)
        {
            // ʵ���������Ĳ˵���������Ϊ����λ����������߹��� Canvas �ϣ�������λ�ã�
            currentContextMenu = Instantiate(contextMenuPrefab, transform);
            currentContextMenu.GetComponent<ItemContextMenu>().Setup(this);
           
        }
    }

    // ����뿪ʱ���������Ĳ˵�
    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentContextMenu != null)
        {
            Destroy(currentContextMenu);
        }
    }
}
