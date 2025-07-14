using System.Collections;
using UnityEngine;
using TMPro;

public class TypingFeedbackDisplay : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public float typingSpeed = 0.05f;

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeMessage(message));
    }

    IEnumerator TypeMessage(string message)
    {
        feedbackText.text = "";
        foreach (char c in message.ToCharArray())
        {
            feedbackText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
