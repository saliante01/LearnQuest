using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour
{
    public TextMeshProUGUI[] levelTexts; // 4 textos
    public string[] sceneNames;          // Nombres de las escenas a cargar
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        for (int i = 0; i < levelTexts.Length; i++)
        {
            var text = levelTexts[i];
            if (RectTransformUtility.RectangleContainsScreenPoint(text.rectTransform, mousePos))
            {
                text.color = hoverColor;

                if (Input.GetMouseButtonDown(0) && i < sceneNames.Length)
                {
                    LevelLoader.LoadLevel(sceneNames[i]); // Usamos tu sistema de carga
                }
            }
            else
            {
                text.color = normalColor;
            }
        }
    }
}