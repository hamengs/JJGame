using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/PlayerData")]
public class PlayerData : ScriptableObject
{
    public GameObject playerPrefab;
    public int health;
    public int money;
    public int attackPower;

    public void ResetData()
    {
        health = 100;
        money = 5;
        attackPower = 5;
    }
}
