using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;
    private bool canPick;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&canPick)
        {
            InventoryManager.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UIManager.Instance.ShowEButton(transform.position + new Vector3(0, 2f, 0));
            canPick = true;


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            UIManager.Instance.HideEButton();

        }
        canPick = false;
    }
}
