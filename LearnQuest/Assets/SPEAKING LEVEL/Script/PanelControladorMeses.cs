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

        private readonly string[] _ordenCorrecto =
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

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
            for (var i = 0; i < Portarretratos.Length; i++)
            {
                if (Portarretratos[i].FotoMesActual != null) continue;
                Respuesta.text = "";
                Debug.Log("Faltan imágenes por colocar");
                return;
            }
            
            for (var i = 0; i < Portarretratos.Length; i++)
            {
                if (Portarretratos[i].NombreMesActual == _ordenCorrecto[i]) continue;
                Debug.Log("El orden es incorrecto");
                Respuesta.text = "El orden es incorrecto";
                Respuesta.color = Color.red;
                return;
            }

            Debug.Log("El orden es correcto");
            Respuesta.text = "El orden es correcto";
            Respuesta.color = Color.green;

            _reconocedor.Iniciar();
        }

        private void OnMesReconocido(string mes)
        {
            Debug.Log("Mes reconocido: " + mes);

            foreach (var portarretrato in Portarretratos)
            {
                if (portarretrato.NombreMesActual == mes)
                {
                    portarretrato.CambiarColorVerdoso();
                }
            }

            var todosReconocidos = true;

            foreach (var portarretrato in Portarretratos)
            {
                if (portarretrato.ReconocidoPorVoz) continue;
                todosReconocidos = false;
                break;
            }

            if (!todosReconocidos) return;

            Debug.Log("Todos los meses han sido reconocidos por voz. Actividad finalizada.");
            Respuesta.text = "¡Bien hecho! Has reconocido todos los meses.";
            _reconocedor.Detener();
        }

        private void OnApplicationQuit()
        {
            _reconocedor?.Detener();
        }
    }
}