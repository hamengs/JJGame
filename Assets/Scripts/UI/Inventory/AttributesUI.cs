using TMPro;
using UnityEngine;
using System.Collections;

public class AttributesUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI attackText;

    private IEnumerator Start()
    {
        // 如果 Inspector 中没有赋值，则从子物体中查找
        if (healthText == null)
            healthText = transform.Find("Health").GetComponent<TextMeshProUGUI>();
        if (moneyText == null)
            moneyText = transform.Find("Money").GetComponent<TextMeshProUGUI>();
        if (attackText == null)
            attackText = transform.Find("Attack").GetComponent<TextMeshProUGUI>();

        // 等待直到 PlayerManager.Instance 可用
        while (PlayerManager.Instance == null)
        {
            yield return null;
        }

        // 订阅 PlayerManager 中的数据变化事件
        PlayerManager.Instance.OnPlayerHealthChange += ChangeHealthText;
        PlayerManager.Instance.OnPlayerMoneyChange += ChangeMoneyText;
        PlayerManager.Instance.OnPlayerAttackChange += ChangeAttackText;

        // 初始化 UI，确保显示当前值
        ChangeHealthText(PlayerManager.Instance.playerData.health);
        ChangeMoneyText(PlayerManager.Instance.playerData.money);
        ChangeAttackText(PlayerManager.Instance.playerData.attackPower);
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnPlayerHealthChange -= ChangeHealthText;
            PlayerManager.Instance.OnPlayerMoneyChange -= ChangeMoneyText;
            PlayerManager.Instance.OnPlayerAttackChange -= ChangeAttackText;
        }
    }

    public void ChangeHealthText(int newHealth)
    {
        if (healthText != null)
            healthText.text = "Health: " + newHealth;
    }

    public void ChangeMoneyText(int newMoney)
    {
        if (moneyText != null)
            moneyText.text = "Money: " + newMoney;
    }

    public void ChangeAttackText(int newAttack)
    {
        if (attackText != null)
            attackText.text = "Attack: " + newAttack;
    }
}
