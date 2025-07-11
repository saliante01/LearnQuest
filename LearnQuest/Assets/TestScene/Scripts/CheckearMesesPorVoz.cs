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

    public class CambiarMesesPorVoz : MonoBehaviour
    {
        private KeywordRecognizer _keywordRecognizer;
        private Dictionary<string, Action> _comandosReconocidos;

        void Start()
        {
            var comandosMeses = new List<ComandoVoz>
            {
                new("yanuari", "January"),
                new("februari", "February"),
                new("march", "March"),
                new("april", "April"),
            };

            _comandosReconocidos = new Dictionary<string, Action>();

            foreach (var comando in comandosMeses)
            {
                _comandosReconocidos[comando.Pronunciacion] = () => MarcarMesComoReconocido(comando);
            }


            _keywordRecognizer = new KeywordRecognizer(_comandosReconocidos.Keys.ToArray());
            _keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
            _keywordRecognizer.Start();
        }

        private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
        {
            Debug.Log("Color reconocido: " + args.text);
            _comandosReconocidos[args.text].Invoke();
        }

        private void MarcarMesComoReconocido(ComandoVoz comando)
        {
            if (comando.Reconocido) return;

            comando.Reconocido = true;
            Debug.Log($"Mes reconocido: {comando.Nombre}");
        }

        void OnApplicationQuit()
        {
            if (_keywordRecognizer != null && _keywordRecognizer.IsRunning)
                _keywordRecognizer.Stop();
        }
    }
}