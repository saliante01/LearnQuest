using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioButtonController : MonoBehaviour
{
    [System.Serializable]
    public class AudioButton
    {
        public Button button;
        public AudioSource audioSource;
    }

    public DialogListening dialogListening;               // Referencia al diálogo inicial
    public AudioButton[] audioButtons;                    // Dos botones con audio
    public GameObject imagesPanel;                        // Panel que contiene las 6 imágenes
    public EvaluationManager evaluationManager;
    private void Start()
    {
        // Oculta botones y panel al inicio
        foreach (var ab in audioButtons)
        {
            ab.button.gameObject.SetActive(false);
        }

        if (imagesPanel != null)
            imagesPanel.SetActive(false);

        // Suscribirse al evento cuando termina el diálogo
        if (dialogListening != null)
            dialogListening.OnDialogFinished += ActivateUI;

        // Asignar listeners para reproducir audio
        foreach (var ab in audioButtons)
        {
            ab.button.onClick.AddListener(() => PlayAudio(ab));
        }
    }

    void ActivateUI()
    {
        foreach (var ab in audioButtons)
            ab.button.gameObject.SetActive(true);

        if (imagesPanel != null)
            imagesPanel.SetActive(true);

        // Activar botones Check cuando aparece el resto de la interfaz
        if (evaluationManager != null)
        {
            evaluationManager.checkAudio1Button.gameObject.SetActive(true);
            evaluationManager.checkAudio2Button.gameObject.SetActive(true);
        }
    }

    void PlayAudio(AudioButton ab)
    {
        if (!ab.audioSource.isPlaying)
        {
            ab.audioSource.Play();
            ab.button.interactable = false;
            StartCoroutine(ReenableButtonAfterDelay(ab.button, ab.audioSource.clip.length));
        }
    }

    IEnumerator ReenableButtonAfterDelay(Button button, float delay)
    {
        yield return new WaitForSeconds(delay);
        button.interactable = true;
    }
}
