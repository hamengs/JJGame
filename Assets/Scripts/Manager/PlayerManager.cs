using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerData playerData;
    public GameObject player;
    public GameObject playerPrefab;
    public PlayerController playerController;
    public bool isPlayerDead = false;

    public event Action OnPlayerDeath;
    public event Action<int> OnPlayerHealthChange;
    public event Action<int> OnPlayerMoneyChange;
    public event Action<int> OnPlayerAttackChange;


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject scenePlayer = GameObject.FindGameObjectWithTag("Player");

        if (scenePlayer != null)
        {
            player = scenePlayer;
        }
        else
        {
            if (playerPrefab != null)
            {
                if (SceneController.Instance.currentPortal != null)
                {
                    player = Instantiate(playerPrefab, SceneController.Instance.currentPortal.transform.position, Quaternion.identity);
                }
                else
                {
                    player = Instantiate(playerPrefab);
                }
               
            }
            else
            {
                Debug.LogError("场景没有找到player，也没有预制体引用，检查设置");
            }
        }
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

    }

    public void PlusHealth(int health)
    {
        ChangeHealth(health);
    }

    public void AddMoney(int money)
    {
        ChangeMoney(money);
    }

    public void PlusAttackPower(int power)
    {
        ChangeAttackPower(power);
    }

    public void DamagePlayer(int damage)
    {
        playerData.health -= damage;
        OnPlayerHealthChange?.Invoke(playerData.health);
        if (playerData.health <= 0 && !isPlayerDead)
        {
            isPlayerDead = true;
            OnPlayerDeath?.Invoke();
        }
    }

    public void ChangeHealth(int health)
    {
        playerData.health += health;
        OnPlayerHealthChange?.Invoke(playerData.health);
    }

    public void ChangeMoney(int money)
    {
        playerData.money += money;
        OnPlayerMoneyChange?.Invoke(playerData.money);
    }

    public void ChangeAttackPower(int power)
    {
        playerData.attackPower += power;
        OnPlayerAttackChange?.Invoke(playerData.attackPower);
    }

    public void SetHealth(int health)
    {
        playerData.health = health;
        OnPlayerHealthChange?.Invoke(playerData.health);
    }

    public void SetMoney(int money)
    {
        playerData.money = money;
        OnPlayerMoneyChange?.Invoke(playerData.money);
    }

    public void SetAttackPower(int power)
    {
        playerData.attackPower = power;
        OnPlayerAttackChange?.Invoke(playerData.attackPower);
    }


}
