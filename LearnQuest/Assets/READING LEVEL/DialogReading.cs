using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogReading : MonoBehaviour
{
    [Header("Configuración del diálogo")]
    public TextMeshProUGUI textoDialogo;
    [TextArea(3, 10)]
    public List<string> lineasDialogo;
    public float velocidadEscritura = 0.05f;
    public float pausaEntreLineas = 1.5f;

    [Header("Estado del diálogo")]
    public GameObject UIActividad;
    private int indiceLineaActual = 0;

    void Start()
    {
        if (lineasDialogo != null && lineasDialogo.Count > 0)
        {
            StartCoroutine(MostrarDialogo());
        }
        else
        {
            Debug.LogWarning("No se han asignado líneas de diálogo.");
        }
    }

    IEnumerator MostrarDialogo()
    {
        while (indiceLineaActual < lineasDialogo.Count)
        {
            yield return StartCoroutine(EscribirLinea(lineasDialogo[indiceLineaActual]));
            indiceLineaActual++;
            yield return new WaitForSeconds(pausaEntreLineas);
        }

        
        UIActividad.SetActive(true);

       
    }

    IEnumerator EscribirLinea(string linea)
    {
        textoDialogo.text = "";
        foreach (char letra in linea)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }



    public void ContinuarButon() {

        LevelLoader.LoadLevel("SampleScene");
    
    
    }
}
