using System.Collections;
using UnityEngine;
using TMPro;

public class DialogListening : MonoBehaviour
{
    [Header("Configuración del diálogo")]
    public TextMeshProUGUI dialogText;
    public string[] dialogLines;
    public float typingSpeed = 0.05f;
    public float waitAfterLine = 1f; // ← tiempo de espera entre líneas

    private int currentLineIndex = 0;
    public bool isDialogueFinished = false;

    void Start()
    {
        if (dialogLines.Length > 0)
        {
            StartCoroutine(PlayDialogue());
        }
    }

    IEnumerator PlayDialogue()
    {
        while (currentLineIndex < dialogLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogLines[currentLineIndex]));
            yield return new WaitForSeconds(waitAfterLine);
            currentLineIndex++;
        }

        dialogText.text = "";
        isDialogueFinished = true;
    }

    IEnumerator TypeLine(string line)
    {
        dialogText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
