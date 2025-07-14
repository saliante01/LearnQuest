using System.Collections;
using Assets.TestScene.Scripts;
using TMPro;
using UnityEngine;

namespace Assets.SPEAKING_LEVEL.Script
{
    public class PanelControladorMeses : MonoBehaviour
    {
        public Portarretrato[] Portarretratos;
        public TextMeshProUGUI Respuesta;
        private CheckearMesesPorVoz _reconocedor;
        public TextMeshProUGUI inicioDeVoz;
        public GameObject botonMenu;
        
        private const string MesFinalCorrecto = "January";

        private readonly string[] _ordenCorrecto =
        {
            "January", "February", "March", "April"
        };

        private int _indiceMesActual;

        private void Start()
        {
            Respuesta.text = "";
            foreach (var portarretrato in Portarretratos)
            {
                portarretrato.OnFotoMesAsignada += ValidarOrden;
            }

            _reconocedor = new CheckearMesesPorVoz();
            _reconocedor.OnMesReconocido += OnMesReconocido;
        }

        public void ValidarOrden()
        {
            StartCoroutine(ValidarOrdenCoroutine());
        }

        private IEnumerator ValidarOrdenCoroutine()
        {
            for (var i = 0; i < Portarretratos.Length; i++)
            {
                if (Portarretratos[i].FotoMesActual != null) continue;
                Respuesta.text = "";
                Debug.Log("Faltan imágenes por colocar");
                yield break;
            }

            for (var i = 0; i < Portarretratos.Length; i++)
            {
                if (Portarretratos[i].NombreMesActual == _ordenCorrecto[i]) continue;
                Debug.Log("El orden es incorrecto");
                Respuesta.text = "I think that's not the right order.";
                yield break;
            }

            Debug.Log("El orden es correcto");
            
            yield return StartCoroutine(MostrarMensajeYEsperar($"Good job! That looks like the right order.", 
                "Now tell me the names. I'm searching for the one that begins with a 'y' sound."));
            
            foreach (var t in Portarretratos)
            {
                t.DesabilitarArrastre();
            }

            _indiceMesActual = 0;
            yield return new WaitForSeconds(1.5f);
            inicioDeVoz.text = $"Say the month: {_ordenCorrecto[_indiceMesActual]}";
            _reconocedor.Iniciar();
            Debug.Log("reconocedor iniciado");
        }

        private void OnMesReconocido(string mes)
        {
            Debug.Log("Mes reconocido: " + mes);

            if (!EsMesEsperado(mes))
            {
                StartCoroutine(MostrarMensajeYEsperar($"That's not the sound of the correct name.", 
                    $"Try again: {_ordenCorrecto[_indiceMesActual]}"));
                return;
            }

            ProcesarMesCorrecto(mes);
            _indiceMesActual++;
            ActualizarEstadoTrasReconocimiento();
        }

        private IEnumerator MostrarMensajeYEsperar(string primerMensaje, string segundoMensaje)
        {
            yield return StartCoroutine(MostrarTextoComoDialogo(inicioDeVoz, primerMensaje));
            yield return new WaitForSeconds(1f); // pequeña pausa entre mensajes
            yield return StartCoroutine(MostrarTextoComoDialogo(inicioDeVoz, segundoMensaje));
        }

        private bool EsMesEsperado(string mes)
        {
            bool esperado = mes == _ordenCorrecto[_indiceMesActual];
            if (!esperado)
            {
                Debug.Log($"Se esperaba {_ordenCorrecto[_indiceMesActual]}, pero se dijo {mes}");
            }
            return esperado;
        }

        private void ProcesarMesCorrecto(string mes)
        {
            foreach (var portarretrato in Portarretratos)
            {
                if (portarretrato.NombreMesActual == mes && !portarretrato.ReconocidoPorVoz)
                {
                    portarretrato.CambiarColorVerdoso();
                    portarretrato.ReconocidoPorVoz = true;
                }
            }
        }

        private void ActualizarEstadoTrasReconocimiento()
        {
            if (_indiceMesActual >= _ordenCorrecto.Length)
            {
                Debug.Log("Todos los meses han sido reconocidos por voz.");
                inicioDeVoz.text = "Nice job! You tell all the months.";
                StartCoroutine(IniciarFaseFinal());
            }
            else
            {
                inicioDeVoz.text = $"Nice job. Now say the month: {_ordenCorrecto[_indiceMesActual]}";
            }
        }

        private IEnumerator IniciarFaseFinal()
        {
            yield return new WaitForSeconds(1.5f);
            inicioDeVoz.text = "Tell me which month is best for me.";

            _reconocedor.Detener();
            _reconocedor.ResetearReconocidos();
            _reconocedor.OnMesReconocido -= OnMesReconocido;
            _reconocedor.OnMesReconocido += OnMesCorrectoReconocido;
            _reconocedor.Iniciar();
        }

        private void OnMesCorrectoReconocido(string mes)
        {
            if (mes != MesFinalCorrecto)
            {
                StartCoroutine(MostrarMensajeYEsperar(
                    "That's not the sound of the correct name.",
                    "Try again."
                ));
                _reconocedor.ResetearReconocidos();
                return;
            }

            StartCoroutine(MostrarTextoComoDialogo(inicioDeVoz, "Perfect! Thank you very much."));

            Debug.Log("Actividad finalizada: Mes final reconocido correctamente.");
            _reconocedor.Detener();
            botonMenu.SetActive(true);
        }

        private void OnApplicationQuit()
        {
            _reconocedor.OnMesReconocido -= OnMesReconocido;
            _reconocedor.OnMesReconocido -= OnMesCorrectoReconocido;
            _reconocedor?.Detener();
        }

        private IEnumerator MostrarTextoComoDialogo(TextMeshProUGUI textoUI, string mensaje, float velocidad = 0.05f)
        {
            textoUI.text = "";
            foreach (char letra in mensaje)
            {
                textoUI.text += letra;
                yield return new WaitForSeconds(velocidad);
            }
        }
    }
}