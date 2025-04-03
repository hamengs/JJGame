using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerData playerData;
    public PlayerController playerController;
    public bool isPlayerDead = false;

    public event Action OnPlayerDeath;


    private void Awake()
    {
        base.Awake();
        playerData = ScriptableObject.Instantiate(playerData);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerDead == true) 
        {
            isPlayerDead = false;
            OnPlayerDeath?.Invoke();
        }
    }

    public void PlusHealth(int health)
    {
        playerData.health += health;
        UIManager.Instance.SetHealthValue(playerData.health);
        playerController.RefreshPlayerData();
    }

    public void AddMoney(int money)
    {
        playerData.money += money;
        UIManager.Instance.SetMoneyValue(playerData.money);
        playerController.RefreshPlayerData();
    }

    public void PlusAttackPower(int power)
    {
        playerData.attackPower += power;
        UIManager.Instance.SetAttackValue(playerData.attackPower);
        playerController.RefreshPlayerData();
    }

    public void DamagePlayer(int damage)
    {
        playerController.Damage(damage);
        if (playerData.health <= 0 && !isPlayerDead)
        {
            isPlayerDead = true;
            OnPlayerDeath?.Invoke();
        }
    }
}
