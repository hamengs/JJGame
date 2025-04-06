using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCData npcData;
    private bool canTalk;
    public bool hasShop;

    private void Start()
    {
        if (npcData.shopdata != null)
        {
            hasShop = true;
        }
        else
        {
            hasShop = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTalk == true)
        {
            //有商店的时候才显示shop按钮
            if (!hasShop)
            {
                UIManager.Instance.chatAndShop.shopButton.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
                UIManager.Instance.chatAndShop.shopButton.gameObject.GetComponent<CanvasGroup>().interactable = false;
                UIManager.Instance.chatAndShop.shopButton.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                UIManager.Instance.chatAndShop.shopButton.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
                UIManager.Instance.chatAndShop.shopButton.gameObject.GetComponent<CanvasGroup>().interactable = true;
                UIManager.Instance.chatAndShop.shopButton.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

                UIManager.Instance.ShowChat();

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canTalk = true;
            UIManager.Instance.chatAndShop.npcData = this.npcData;
            UIManager.Instance.ShowEButton(transform.position+new Vector3(0,3f,0));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canTalk = false;
            UIManager.Instance.HideEButton();
            UIManager.Instance.HideChat();
            UIManager.Instance.chatAndShop.npcData = null;
        }
    }
}
