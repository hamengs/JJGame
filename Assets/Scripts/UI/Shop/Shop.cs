using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "ShopScript/Shop")]
public class Shop : ScriptableObject
{
    public string shopName;
    public Item[] items;

   
}
