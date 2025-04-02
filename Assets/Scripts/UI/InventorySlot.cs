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
    // 鼠标进入时显示上下文菜单
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (contextMenuPrefab != null && currentContextMenu == null)
        {
            // 实例化上下文菜单，可以作为本槽位的子物体或者挂在 Canvas 上（并调整位置）
            currentContextMenu = Instantiate(contextMenuPrefab, transform);
            currentContextMenu.GetComponent<ItemContextMenu>().Setup(this);
           
        }
    }

    // 鼠标离开时隐藏上下文菜单
    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentContextMenu != null)
        {
            Destroy(currentContextMenu);
        }
    }
}
