using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleDialogManager : MonoBehaviour
{
    [Header("UI Inicial")]
    public GameObject  TextoComplementario;
    public TextMeshProUGUI initialDialogText;
    public Button continueButton;
    public GameObject segundoPanel;
    [Header("UI de Evaluación")]
    public GameObject infoImagePanel;
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TMP_InputField inputField3;
    public Button validateButton;

    [Header("Mensajes")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    [Header("Respuestas Correctas")]
    public string correctWord1 = "John";
    public string correctWord2 = "London";
    public string correctWord3 = "25";

    [Header("Texto de introducción")]
    [TextArea(2, 5)]
    public string[] introLines;
    public float typingSpeed = 0.05f;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private int failCounter = 0;

    public GameObject volverMenuBoton;
   
    void Start()
    {
        
        infoImagePanel.SetActive(false);
        messagePanel.SetActive(true); 
        continueButton.gameObject.SetActive(false);
        validateButton.gameObject.SetActive(false); 
        TextoComplementario.SetActive(false);
        continueButton.onClick.AddListener(ShowInformationCard);
        validateButton.onClick.AddListener(ValidateAnswers);
        segundoPanel.SetActive(false);
        StartCoroutine(PlayIntroDialog());

        inputField1.onValueChanged.AddListener(delegate { CheckInputFields(); });
        inputField2.onValueChanged.AddListener(delegate { CheckInputFields(); });
        inputField3.onValueChanged.AddListener(delegate { CheckInputFields(); });

        inputField1.gameObject.SetActive(false);
        inputField2.gameObject.SetActive(false);
        inputField3.gameObject.SetActive(false);

    }

    IEnumerator PlayIntroDialog()
    {
        while (currentLineIndex < introLines.Length)
        {
            yield return StartCoroutine(TypeLine(introLines[currentLineIndex]));
            currentLineIndex++;
            yield return new WaitForSeconds(0.5f);
        }

        continueButton.gameObject.SetActive(true);
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        initialDialogText.text = "";

        foreach (char c in line)
        {
            initialDialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    void CheckInputFields()
    {
        bool allFilled = !string.IsNullOrWhiteSpace(inputField1.text) &&
                         !string.IsNullOrWhiteSpace(inputField2.text) &&
                         !string.IsNullOrWhiteSpace(inputField3.text);

        validateButton.gameObject.SetActive(allFilled);
    }
    void ShowInformationCard()
    {
      
        TextoComplementario.SetActive(true);
        infoImagePanel.SetActive(true);
       
        continueButton.gameObject.SetActive(false);
        segundoPanel.SetActive(true);
        inputField1.gameObject.SetActive(true);
        inputField2.gameObject.SetActive(true);
        inputField3.gameObject.SetActive(true);

        

    }

    void ValidateAnswers()
    {
        string word1 = inputField1.text.Trim();
        string word2 = inputField2.text.Trim();
        string word3 = inputField3.text.Trim();

        if (word1.Equals(correctWord1, System.StringComparison.OrdinalIgnoreCase) &&
            word2.Equals(correctWord2, System.StringComparison.OrdinalIgnoreCase) &&
            word3.Equals(correctWord3, System.StringComparison.OrdinalIgnoreCase))
        {
            ShowMessage("¡Excelent! You've been a great help. Now you can move on to the next one.");
            volverMenuBoton.SetActive(true);
            validateButton.gameObject.SetActive(false);
        }
        else
        {
            failCounter++;
            ShowMessage("I think something isn't right. Try again!");
            Debug.Log("Fallos acumulados: " + failCounter);
        }
    }

    void ShowMessage(string msg)
    {
        
        messagePanel.SetActive(true);
        initialDialogText.text = msg;
        CancelInvoke(nameof(HideMessage));
    }

    void HideMessage()
    {
        // messagePanel.SetActive(false);
        initialDialogText.text = ""; 
    }

    public void VolverMenu() {
        LevelLoader.LoadLevel("SampleScene");
    }
}
