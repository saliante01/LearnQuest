using System.Collections;
using Assets.SPEAKING_LEVEL.Script;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.READING_LEVEL
{
    public class ContenedorRespuesta : MonoBehaviour, IDropHandler
    {
        public delegate void RespuestaAsignadaHandler(string texto);
        public event RespuestaAsignadaHandler OnRespuestaAsignada;
        
        [SerializeField] private TextMeshProUGUI _textoRespuestaUi;
        
        private GameObject _objetoActual;
    
        public string TextoRespuesta => _textoRespuestaUi.text;
        

        public void OnDrop(PointerEventData eventData)
        {
            var objetoArrastrado = eventData.pointerDrag;
            if (!EsObjetoValido(objetoArrastrado)) return;

            AsignarRespuesta(objetoArrastrado);
            StartCoroutine(LiberarConDelay());
        }

        private bool EsObjetoValido(GameObject objeto)
        {
            if (objeto == null) return false;
            return objeto.GetComponentInChildren<TextMeshProUGUI>() != null;
        }

        private void AsignarRespuesta(GameObject nuevoObjeto)
        {
            var display = nuevoObjeto.GetComponentInChildren<TextMeshProUGUI>();
            _textoRespuestaUi.text = display.text;
            _objetoActual = nuevoObjeto;
            
            nuevoObjeto.GetComponent<CanvasGroup>().alpha = 0f;
            nuevoObjeto.transform.SetParent(transform);
        }
        
        private IEnumerator LiberarConDelay()
        {
            yield return new WaitForSeconds(0.7f);
            LiberarObjeto();
        }

        private void LiberarObjeto()
        {
            if (_objetoActual == null) return;
            
            _objetoActual.GetComponent<CanvasGroup>().alpha = 1f;
            _textoRespuestaUi.text = "";
            
            var anteriorArrastrable = _objetoActual.GetComponent<ImagenArrastrable>();
            if (anteriorArrastrable == null) return;
            
            _objetoActual.SetActive(true);
            anteriorArrastrable.RestaurarAOrigen();

            OnRespuestaAsignada?.Invoke(TextoRespuesta);
            _objetoActual = null;
        }
    }
}