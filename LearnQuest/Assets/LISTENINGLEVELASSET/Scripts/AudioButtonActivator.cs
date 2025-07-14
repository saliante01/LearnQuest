using UnityEngine;
using UnityEngine.UI;
public class AudioButtonActivator : MonoBehaviour
{
    public DialogListening dialogListening;     // Referencia al script del diálogo
    public GameObject audioButtonObject;        // El botón que se activa
    public AudioSource audioSource;             // Audio que se reproduce
    public GameObject UIActiviti1;         // ← NUEVO: objeto que se activa una vez

    private Button audioButton;
    private bool hasActivatedObject = false;    // ← NUEVO: control de activación única

    void Start()
    {
        audioButton = audioButtonObject.GetComponent<Button>();
        audioButtonObject.SetActive(false);
        audioButton.onClick.AddListener(PlayAudio);

        if (UIActiviti1 != null)
            UIActiviti1.SetActive(false); // Opcional: empieza desactivado
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

            // Activar el objeto solo la primera vez
            if (!hasActivatedObject && UIActiviti1 != null)
            {
                UIActiviti1.SetActive(true);
                hasActivatedObject = true;
            }

            Invoke(nameof(EnableButton), audioSource.clip.length);
        }
    }

    void EnableButton()
    {
        audioButton.interactable = true;
    }
}