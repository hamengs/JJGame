using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.inventoryPanel = transform.Find("InventoryPanel").GetComponent<Transform>();
        UIManager.Instance.inventoryGameObject = this.gameObject;
        UIManager.Instance.closeButton = transform.Find("CloseButton").GetComponent<Button>();
        UIManager.Instance.inventoryGroup = GetComponent<CanvasGroup>();

        UIManager.Instance.closeButton.onClick.AddListener(UIManager.Instance.HideInventory);
    }


}
