using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class CambiarColorPorVoz : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> comandosDeColor;

    void Start()
    {
        comandosDeColor = new Dictionary<string, Action>()
        {
            { "grin", () => CambiarColor(Color.red) },
            { "verde", () => CambiarColor(Color.green) },
            { "azul", () => CambiarColor(Color.blue) },
            { "amarillo", () => CambiarColor(Color.yellow) },
            { "blanco", () => CambiarColor(Color.white) },
            { "negro", () => CambiarColor(Color.black) },
            { "magenta", () => CambiarColor(Color.magenta) },
            { "cian", () => CambiarColor(Color.cyan) },
            { "gris", () => CambiarColor(Color.gray) }
        };

        keywordRecognizer = new KeywordRecognizer(comandosDeColor.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
        keywordRecognizer.Start();
    }

    private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Color reconocido: " + args.text);
        comandosDeColor[args.text].Invoke();
    }

    private void CambiarColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void OnApplicationQuit()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
            keywordRecognizer.Stop();
    }
}
