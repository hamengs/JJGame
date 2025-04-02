using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UIManager.Instance.AddItemToInventory(item);
            Destroy(gameObject);
        }
    }
}
