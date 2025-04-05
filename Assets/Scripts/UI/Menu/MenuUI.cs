using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

    public Button startButton;
    public Button continueButton;
    public Button exitButton;
    public TextMeshProUGUI continueButtonText;
    public Image continueButtonImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveManager.Instance.LoadGame();

        startButton = transform.Find("Start").GetComponent<Button>();
        continueButton = transform.Find("Continue").GetComponent<Button>();
        exitButton = transform.Find("Exit").GetComponent<Button>();

        startButton.onClick.AddListener(UIManager.Instance.OnStartButtonClicked);
        continueButton.onClick.AddListener(UIManager.Instance.OnContinueButtonClicked);
        exitButton.onClick.AddListener(UIManager.Instance.OnExitButtonClicked);


        SetContinueButton();



    }



    public void SetContinueButton()
    {

        if (string.IsNullOrEmpty(SaveManager.Instance.savedScene))
        {
            continueButtonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            continueButtonImage = continueButton.gameObject.GetComponent<Image>();
            Color c = continueButtonText.color;
            c.a = 0.4f;
            continueButtonText.color = c;

            Color I = continueButtonImage.color;
            I.a = 0;
            continueButtonImage.color = I;

            continueButton.interactable = false;
        }
        else if(!string.IsNullOrEmpty(SaveManager.Instance.savedScene))
        {
            TextMeshProUGUI text = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            Color c = text.color;
            c.a = 1f;
            text.color = c;

            continueButton.interactable = true;
        }
    }
}
