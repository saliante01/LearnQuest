using UnityEngine;
using UnityEngine.UI;
public class AudioButtonActivator : MonoBehaviour
{
    public DialogListening dialogListening;     // Referencia al script de diálogo
    public GameObject audioButtonObject;        // Botón que se activará
    public AudioSource audioSource;             // Componente que reproduce el audio

    private Button audioButton;

    void Start()
    {
        audioButton = audioButtonObject.GetComponent<Button>();
        audioButtonObject.SetActive(false); // Ocultar el botón al inicio
        audioButton.onClick.AddListener(PlayAudio);
    }

    void Update()
    {
        if (dialogListening.isDialogueFinished && !audioButtonObject.activeSelf)
        {
            audioButtonObject.SetActive(true);
        }
    }

    void PlayAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioButton.interactable = false;
            Invoke(nameof(EnableButton), audioSource.clip.length);
        }
    }

    void EnableButton()
    {
        audioButton.interactable = true;
    }
}