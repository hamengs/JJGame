using UnityEngine;
using UnityEngine.UI;

public class DeadInformationUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        {
            UIManager.Instance.deadInformation = this.gameObject;
            UIManager.Instance.restartButton = transform.Find("Restart").GetComponent<Button>();
            UIManager.Instance.exitButton = transform.Find("Exit").GetComponent<Button>();
            UIManager.Instance.deadInformationGroup = GetComponent<CanvasGroup>();

            UIManager.Instance.exitButton.onClick.AddListener(UIManager.Instance.OnExitButtonClicked);
            UIManager.Instance.restartButton.onClick.AddListener(UIManager.Instance.OnRestartButtonClicked);
        }

    }
}
