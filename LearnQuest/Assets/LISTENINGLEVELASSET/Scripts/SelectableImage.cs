using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableImage : MonoBehaviour, IPointerClickHandler
{
    public int imageIndex;
    public bool isAudio1;
    public ImageSelectionManager selectionManager;

    private Image imageComponent;
    private Color originalColor;
    public Color selectedColor = Color.yellow;
    public Color correctColor = Color.green;

    private bool isSelected = false;
    private bool isCorrect = false;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        originalColor = imageComponent.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCorrect) return;

        // Primero limpiamos los colores del grupo
        selectionManager.ResetColorsInGroup(isAudio1);

        // Luego actualizamos la selección
        selectionManager.OnImageSelected(isAudio1, imageIndex);

        // Luego pintamos esta imagen como seleccionada
        MarkSelected();

        Debug.Log($"Imagen seleccionada: index={imageIndex}, grupo={(isAudio1 ? "Audio1" : "Audio2")}");
    }

    public void MarkSelected()
    {
        if (imageComponent != null)
        {
            imageComponent.color = selectedColor;
            isSelected = true;
        }
    }

    public void MarkCorrect()
    {
        if (imageComponent != null)
        {
            imageComponent.color = correctColor;
            isCorrect = true;
        }
    }

    public void ResetSelection()
    {
        if (!isCorrect && imageComponent != null)
        {
            imageComponent.color = originalColor;
            isSelected = false;
        }
    }
    public bool IsAudio1() => isAudio1;
    public int Index() => imageIndex;
}
