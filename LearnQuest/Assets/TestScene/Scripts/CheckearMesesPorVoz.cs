using System;
using System.Collections.Generic;
using System.Linq;
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
        private KeywordRecognizer _keywordRecognizer;
        private Dictionary<string, Action> _comandosReconocidos;
        private List<ComandoVoz> _comandosMeses;

        public event Action<string> OnMesReconocido;

        public CheckearMesesPorVoz()
        {
            _comandosMeses = new List<ComandoVoz>
            {
                new("yenuari", "January"),
                new("februari", "February"),
                new("march", "March"),
                new("eipril", "April"),
            };

            _comandosReconocidos = new Dictionary<string, Action>();

            foreach (var comando in _comandosMeses)
            {
                _comandosReconocidos[comando.Pronunciacion] = () => MarcarMesComoReconocido(comando);
            }
        }

        public void Iniciar()
        {
            if (_keywordRecognizer != null) return;

            _keywordRecognizer = new KeywordRecognizer(_comandosReconocidos.Keys.ToArray());
            _keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
            _keywordRecognizer.Start();
        }

        public void Detener()
        {
            if (_keywordRecognizer != null && _keywordRecognizer.IsRunning)
            {
                _keywordRecognizer.Stop();
                _keywordRecognizer.OnPhraseRecognized -= OnKeywordRecognized;
            }
        }

        private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
        {
            Debug.Log("Mes reconocido: " + args.text);

            if (_comandosReconocidos.TryGetValue(args.text, out var accion))
                accion.Invoke();
            else
                Debug.LogWarning($"Palabra no reconocida: {args.text}");
        }

        private void MarcarMesComoReconocido(ComandoVoz comando)
        {
            if (comando.Reconocido) return;

            comando.Reconocido = true;
            Debug.Log($"Mes reconocido: {comando.Nombre}");
            OnMesReconocido?.Invoke(comando.Nombre);
        }
    }
}
