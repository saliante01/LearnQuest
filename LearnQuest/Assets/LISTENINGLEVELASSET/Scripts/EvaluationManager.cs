using UnityEngine;
using UnityEngine.UI;

public class EvaluationManager : MonoBehaviour
{
    public ImageSelectionManager selectionManager;
    public TypingFeedbackDisplay feedbackDisplay;
    public Button checkAudio1Button;
    public Button checkAudio2Button;
    public Button continueButton;

    private bool audio1Correct = false;
    private bool audio2Correct = false;

    void Start()
    {
        continueButton.gameObject.SetActive(false);
        checkAudio1Button.gameObject.SetActive(false); 
        checkAudio2Button.gameObject.SetActive(false); 
    }


    public void CheckAudio1()
    {
        if (selectionManager.IsAudio1Correct())
        {
            selectionManager.ValidateAudio1();
            feedbackDisplay.ShowMessage("Audio 1: Correct!");
            audio1Correct = true;
        }
        else
        {
            selectionManager.ValidateAudio1(); // A�n resetea si no es correcta
            feedbackDisplay.ShowMessage("Not quite right. Give Audio 1 another try!");
        }

        CheckIfLevelCompleted();
    }

    public void CheckAudio2()
    {
        if (selectionManager.IsAudio2Correct())
        {
            selectionManager.ValidateAudio2();
            feedbackDisplay.ShowMessage("Audio 2: Correct!");
            audio2Correct = true;
        }
        else
        {
            selectionManager.ValidateAudio2(); // A�n resetea si no es correcta
            feedbackDisplay.ShowMessage("Not quite right. Give Audio 2 another try!");
        }

        CheckIfLevelCompleted();
    }

    void CheckIfLevelCompleted()
    {
        if (audio1Correct && audio2Correct)
        {
            feedbackDisplay.ShowMessage("Great job! You've finished the level. Time to move on to the next one!");
            continueButton.gameObject.SetActive(true);
        }
    }
}
