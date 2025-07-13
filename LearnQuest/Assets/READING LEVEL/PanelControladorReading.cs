using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.READING_LEVEL
{
    [System.Serializable]
    public class Pregunta
    {
        public string Texto;
        public string[] Opciones;
        public string RespuestaCorrecta;
    }
    
    [System.Serializable]
    public struct RespuestaDada
    {
        public string Pregunta;
        public string Respuesta;
        public bool Correcta;

        public RespuestaDada(string pregunta, string respuesta, bool correcta)
        {
            Pregunta = pregunta;
            Respuesta = respuesta;
            Correcta = correcta;
        }
    }

    public class PanelControladorReading : MonoBehaviour
    {
        public ContenedorRespuesta ContenedorRespuesta;
        public TextMeshProUGUI Pregunta;
        public TextMeshProUGUI Respuesta1;
        public TextMeshProUGUI Respuesta2;
        public TextMeshProUGUI Respuesta3;

        [SerializeField] private List<Pregunta> _preguntas;
        private readonly List<RespuestaDada> _respuestasDadas = new();

        private int _preguntaActual;

        private void Start()
        {
            _preguntaActual = 0;
            
            _preguntas = new List<Pregunta>
            {
                new()
                {
                    Texto = "What color is Tom’s t-shirt?",
                    Opciones = new[] { "His\nt-shirt is blue.", "His\nt-shirt is red.", "His\nt-shirt is green." },
                    RespuestaCorrecta = "His\nt-shirt is red."
                },
                new()
                {
                    Texto = "Why does Tom wear a jacket?",
                    Opciones = new[] { "Because\nit is cold.", "Because\nit is raining.", "Because\nhe is tired." },
                    RespuestaCorrecta = "Because\nit is cold."
                },
                new()
                {
                    Texto = "What is Tom’s favorite hat color?",
                    Opciones = new[] { "His\nfavorite hat is green.", "His\nfavorite hat is blue.", "His\nfavorite hat is red." },
                    RespuestaCorrecta = "His\nfavorite hat is blue."
                }
            };

            AsignarNuevaPregunta();
            ContenedorRespuesta.OnRespuestaAsignada += OnRespuestaAsignada;
        }

        private void AsignarNuevaPregunta()
        {
            Pregunta.text = _preguntas[_preguntaActual].Texto;
            Respuesta1.text = _preguntas[_preguntaActual].Opciones[0];
            Respuesta2.text = _preguntas[_preguntaActual].Opciones[1];
            Respuesta3.text = _preguntas[_preguntaActual].Opciones[2];
        }

        private void OnRespuestaAsignada(string respuesta)
        {
            var esCorrecta = respuesta == _preguntas[_preguntaActual].RespuestaCorrecta;
            _respuestasDadas.Add(new RespuestaDada(_preguntas[_preguntaActual].Texto, respuesta, esCorrecta));
            _preguntaActual++;
            
            if (_preguntaActual < _preguntas.Count)
            {
                AsignarNuevaPregunta();
            }
            else
            {
                FinalizarActividad();
            }
        }
        
        private void FinalizarActividad()
        {
            Debug.Log("Actividad finalizada. Resultados:");
            foreach (var respuesta in _respuestasDadas)
            {
                Debug.Log($"Pregunta: {respuesta.Pregunta} | Respuesta: {respuesta.Respuesta} | Correcta: {respuesta.Correcta}");
            }

            Pregunta.text = "¡Actividad completada!";
            Respuesta1.text = "";
            Respuesta2.text = "";
            Respuesta3.text = "";
        }
    }
}