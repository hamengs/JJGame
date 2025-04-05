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
        // ��� Inspector ��û�и�ֵ������������в���
        if (healthText == null)
            healthText = transform.Find("Health").GetComponent<TextMeshProUGUI>();
        if (moneyText == null)
            moneyText = transform.Find("Money").GetComponent<TextMeshProUGUI>();
        if (attackText == null)
            attackText = transform.Find("Attack").GetComponent<TextMeshProUGUI>();

        // �ȴ�ֱ�� PlayerManager.Instance ����
        while (PlayerManager.Instance == null)
        {
            yield return null;
        }

        // ���� PlayerManager �е����ݱ仯�¼�
        PlayerManager.Instance.OnPlayerHealthChange += ChangeHealthText;
        PlayerManager.Instance.OnPlayerMoneyChange += ChangeMoneyText;
        PlayerManager.Instance.OnPlayerAttackChange += ChangeAttackText;

        // ��ʼ�� UI��ȷ����ʾ��ǰֵ
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
