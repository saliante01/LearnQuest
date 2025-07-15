using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySharp;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Assets.TestScene.Scripts
{
    [Serializable]
    public class ComandoVoz
    {
        public string Pronunciacion;
        public string Nombre;
        public bool Reconocido;

        public ComandoVoz(string pronunciacion, string nombre)
        {
            Pronunciacion = pronunciacion;
            Nombre = nombre;
            Reconocido = false;
        }
    }

    public class CheckearMesesPorVoz
    {
        private DictationRecognizer _dictationRecognizer;
        private List<ComandoVoz> _comandosMeses;

        public event Action<string> OnMesReconocido;

        public CheckearMesesPorVoz()
        {
            _comandosMeses = new List<ComandoVoz>
            {
                new("january", "January"),
                new("jenewri", "January"),
                new("february", "February"),
                new("februari", "February"),
                new("febuary", "February"),
                new("marx", "March"),
                new("march", "March"),
                new("april", "April"),
            };
        }

        public void Iniciar()
        {
            if (_dictationRecognizer != null) return;

            _dictationRecognizer = new DictationRecognizer();

            _dictationRecognizer.DictationResult += OnDictationResult;
            _dictationRecognizer.DictationComplete += OnDictationComplete;
            _dictationRecognizer.DictationError += (error, hresult) =>
                Debug.LogError($"Dictation error: {error}");

            _dictationRecognizer.Start();
        }

        public void Detener()
        {
            if (_dictationRecognizer is { Status: SpeechSystemStatus.Running })
            {
                _dictationRecognizer.Stop();
                _dictationRecognizer.DictationResult -= OnDictationResult;
                _dictationRecognizer.DictationComplete -= OnDictationComplete;
                _dictationRecognizer.Dispose();
                _dictationRecognizer = null;
            }
        }

        public void ResetearReconocidos()
        {
            foreach (var comando in _comandosMeses)
            {
                comando.Reconocido = false;
            }
        }

        private void OnDictationResult(string text, ConfidenceLevel confidence)
        {
            Debug.Log("Texto dictado: " + text);

            var mejorCoincidencia = _comandosMeses
                .Select(c => new
                {
                    Comando = c,
                    Puntaje = Fuzz.PartialRatio(text.ToLower(), c.Pronunciacion.ToLower())
                })
                .OrderByDescending(x => x.Puntaje)
                .FirstOrDefault();

            if (mejorCoincidencia != null && mejorCoincidencia.Puntaje >= 65) 
            {
                if (!mejorCoincidencia.Comando.Reconocido)
                {
                    mejorCoincidencia.Comando.Reconocido = true;
                    Debug.Log($"Coincidencia fuzzy: {mejorCoincidencia.Comando.Nombre} ({mejorCoincidencia.Puntaje})");
                    OnMesReconocido?.Invoke(mejorCoincidencia.Comando.Nombre);
                }
            }
            else
            {
                Debug.LogWarning("No se encontró una coincidencia lo suficientemente buena.");
            }
        }

        private void OnDictationComplete(DictationCompletionCause cause)
        {
            Debug.Log("Dictation complete: " + cause);
            if (cause == DictationCompletionCause.Complete)
            {
                _dictationRecognizer?.Start();
            }
        }
    }
}
