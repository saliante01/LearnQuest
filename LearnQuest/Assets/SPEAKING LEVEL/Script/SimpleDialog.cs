using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Interaction
{
    public string[] dialogLinesBefore;  // Diálogos previos a esta interacción
    public GameObject objectToColor;
    public Color targetColor = Color.yellow;
    public string expectedName;
    public int maxAttempts = 3;
}

public class SimpleDialog : MonoBehaviour
{
    public Interaction[] interactions;

    public TextMeshProUGUI dialogText;
    public Button continueButton;

    public TMP_InputField inputField;
    public TextMeshProUGUI feedbackText;

    public float typingSpeed = 0.05f;

    private int currentDialogLine = 0;
    private int currentInteractionIndex = 0;
    private int attempts = 0;
    private bool isTyping = false;
    private bool waitingForInput = false;

    void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        continueButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        ShowNextDialogLine();
    }

    void ShowNextDialogLine()
    {
        if (currentInteractionIndex >= interactions.Length)
        {
            dialogText.text = "¡Has terminado todas las interacciones!";
            continueButton.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            feedbackText.gameObject.SetActive(false);
            return;
        }

        Interaction currentInteraction = interactions[currentInteractionIndex];

        if (currentDialogLine < currentInteraction.dialogLinesBefore.Length)
        {
            StartCoroutine(TypeLine(currentInteraction.dialogLinesBefore[currentDialogLine]));
        }
        else
        {
            // Terminar diálogo previo, activar la interacción
            ActivateInteraction(currentInteraction);
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogText.text = "";
        continueButton.gameObject.SetActive(false);

        foreach (char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueButton.gameObject.SetActive(true);
    }

    void OnContinueClicked()
    {
        if (isTyping) return;
        if (waitingForInput) return; // evitar avanzar mientras espera input

        currentDialogLine++;
        ShowNextDialogLine();
    }

    void ActivateInteraction(Interaction interaction)
    {
        // Cambiar color objeto
        if (interaction.objectToColor != null)
        {
            Renderer rend = interaction.objectToColor.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = interaction.targetColor;
        }

        // Preparar UI input
        continueButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        feedbackText.text = "";

        attempts = 0;
        waitingForInput = true;

        inputField.text = "";
        inputField.ActivateInputField();

        inputField.onSubmit.RemoveAllListeners();
        inputField.onSubmit.AddListener(ValidateInput);
    }

    void ValidateInput(string userInput)
    {
        if (!waitingForInput) return;

        Interaction currentInteraction = interactions[currentInteractionIndex];

        if (userInput.Trim().ToLower() == currentInteraction.expectedName.ToLower())
        {
            feedbackText.text = "¡Correcto!";
            waitingForInput = false;
            inputField.gameObject.SetActive(false);

            inputField.onSubmit.RemoveAllListeners();

            // Reiniciar diálogo para la siguiente interacción
            currentInteractionIndex++;
            currentDialogLine = 0;

            // Esperamos un momento para mostrar el mensaje "correcto" y luego continuar
            StartCoroutine(ContinueAfterDelay(1.5f));
        }
        else
        {
            attempts++;
            feedbackText.text = $"Creo que no se escribe así. Intentos: {attempts}";

            if (attempts >= currentInteraction.maxAttempts)
            {
                feedbackText.text += "\nHas alcanzado el máximo de intentos.";
                // Aquí podrías manejar qué pasa cuando se superan los intentos (por ej. pasar automáticamente)
                // Por ahora permite seguir intentando si quiere.
            }

            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    IEnumerator ContinueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        feedbackText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
    }
}