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
            feedbackDisplay.ShowMessage("Audio 1: ¡Correcto!");
            audio1Correct = true;
        }
        else
        {
            selectionManager.ValidateAudio1(); // Aún resetea si no es correcta
            feedbackDisplay.ShowMessage("Audio 1: Incorrecto. Intenta de nuevo.");
        }

        CheckIfLevelCompleted();
    }

    public void CheckAudio2()
    {
        if (selectionManager.IsAudio2Correct())
        {
            selectionManager.ValidateAudio2();
            feedbackDisplay.ShowMessage("Audio 2: ¡Correcto!");
            audio2Correct = true;
        }
        else
        {
            selectionManager.ValidateAudio2(); // Aún resetea si no es correcta
            feedbackDisplay.ShowMessage("Audio 2: Incorrecto. Intenta de nuevo.");
        }

        CheckIfLevelCompleted();
    }

    void CheckIfLevelCompleted()
    {
        if (audio1Correct && audio2Correct)
        {
            feedbackDisplay.ShowMessage("Perfecto, has completado el nivel. Ahora puedes continuar al siguiente nivel.");
            continueButton.gameObject.SetActive(true);
        }
    }
}
