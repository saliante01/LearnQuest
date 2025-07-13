using System.Collections;
using UnityEngine;
using TMPro;
using Assets.SPEAKING_LEVEL.Script;
public class Dialog : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogText;
    public GameObject retrato1;
    public GameObject retrato2;
    public GameObject retrato3;
    public GameObject retrato4;
    public GameObject Marco1;
    public GameObject Marco2;
    public GameObject Marco3;
    public GameObject Marco4;
    [Header("Diálogo")]
    [TextArea]
    public string[] dialogos;

    [Header("Objeto a Activar al Final")]
    public GameObject BotonContinuar;
    private int index = 0;

    void Start()
    {
        if (dialogos.Length > 0)
        {
            StartCoroutine(MostrarDialogo());
        }
    }

    void Update()
    {
        
    }

    IEnumerator MostrarDialogo()
    {
        foreach (string dialogo in dialogos)
        {
            dialogText.text = "";
            foreach (char letra in dialogo)
            {
                dialogText.text += letra;
                yield return new WaitForSeconds(0.05f); // 1 segundo por letra
            }

            yield return new WaitForSeconds(0.5f); // Pausa antes del siguiente diálogo
        }

        if (BotonContinuar != null)
        {
            BotonContinuar.SetActive(true);
        }
    }


    public void MostrarActividad()
    {
        retrato1.SetActive(true);
        retrato2.SetActive(true);
        retrato3.SetActive(true);
        retrato4.SetActive(true);
        Marco1.SetActive(true);
        Marco2.SetActive(true);
        Marco3.SetActive(true);
        Marco4.SetActive(true);
        BotonContinuar.SetActive(false);
    }

    public void CambiarEscena()
    {


        LevelLoader.LoadLevel("SampleScene");
    }
}
