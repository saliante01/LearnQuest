using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.READING_LEVEL
{
    [System.Serializable]
    public class Pregunta
    {
        public string Texto;
        public Sprite[] Opciones;
        public int RespuestaCorrectaIndex;
    }

    public class PanelControladorReading : MonoBehaviour
    {
        public ContenedorRespuesta ContenedorRespuesta;
        public TextMeshProUGUI Pregunta;
        public Image Respuesta1;
        public Image Respuesta2;
        public Image Respuesta3;
        public TextMeshProUGUI mainDialogSystemInfo;
        public List<Pregunta> Preguntas;

        private int _preguntaActual;
        private bool _respuestaFueCorrecta;
        public GameObject continuarButton;
        private void Start()
        {
            _preguntaActual = 0;
            AsignarNuevaPregunta();
            ContenedorRespuesta.OnRespuestaAsignada += OnRespuestaAsignada;
            ContenedorRespuesta.OnRespuestaLiberada += OnRespuestaLiberada;
        }

        private void AsignarNuevaPregunta()
        {
            Pregunta.text = Preguntas[_preguntaActual].Texto;
            Respuesta1.sprite = Preguntas[_preguntaActual].Opciones[0];
            Respuesta2.sprite = Preguntas[_preguntaActual].Opciones[1];
            Respuesta3.sprite = Preguntas[_preguntaActual].Opciones[2];
            Debug.Log(Respuesta1.sprite.name);
            Debug.Log(Respuesta2.sprite.name);
            Debug.Log(Respuesta3.sprite.name);
        }

        private void OnRespuestaAsignada(Sprite respuesta)
        {
            if (EsRespuestaCorrecta(respuesta))
            {
                _respuestaFueCorrecta = true;
            }
            else
            {
                mainDialogSystemInfo.text = "Oops creo que esa no es la respuesta, vuelve a intentarlo";
                Debug.Log("Oops creo que esa no es la respuesta, vuelve a intentarlo");
                _respuestaFueCorrecta = false;
            }
        }
        
        private bool EsRespuestaCorrecta(Sprite respuesta)
        {
            var preguntaActual = Preguntas[_preguntaActual];
            var respuestaCorrecta = preguntaActual.Opciones[preguntaActual.RespuestaCorrectaIndex];
            return respuesta == respuestaCorrecta;
        }
        
        private void OnRespuestaLiberada()
        {
            if (!_respuestaFueCorrecta) return;
            AvanzarOAprobarActividad();
            _respuestaFueCorrecta = false;
        }

        private void AvanzarOAprobarActividad()
        {
            if (_preguntaActual + 1 < Preguntas.Count)
            {
                _preguntaActual++;
                AsignarNuevaPregunta();
            }
            else
            {
                FinalizarActividad();
            }
        }
        
        private void FinalizarActividad()
        {
            Debug.Log("Actividad finalizada.");
            Pregunta.text = "¡Actividad completada!";
            mainDialogSystemInfo.text = "Muy bien! Haz completado el nivel, presiona continuar para volver al menu";
            _preguntaActual = 0; 
            continuarButton.SetActive(true);
        }


    }
}