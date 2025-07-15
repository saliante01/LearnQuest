using System.Collections;
using Assets.SPEAKING_LEVEL.Script;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.READING_LEVEL
{
    public class ContenedorRespuesta : MonoBehaviour, IDropHandler
    {
        public delegate void RespuestaAsignadaHandler(Sprite sprite);
        public event RespuestaAsignadaHandler OnRespuestaAsignada;
        
        public delegate void RespuestaLiberadaHandler();
        public event RespuestaLiberadaHandler OnRespuestaLiberada;

        
        [SerializeField] public Image imagenRespuesta;
        
        
        private GameObject _objetoActual;

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
            return objeto.GetComponentInChildren<Image>() != null;
        }

        private void AsignarRespuesta(GameObject nuevoObjeto)
        {
            var hijoImagen = nuevoObjeto.transform.Find("Imagen");
            if (hijoImagen == null)
            {
                Debug.LogError("No se encontró el hijo llamado 'Imagen' en el objeto arrastrado.");
                return;
            }
            
            var display = hijoImagen.GetComponent<Image>();
            Debug.Log($"Nombre imagen: {display.sprite.name}");
            
            _objetoActual = nuevoObjeto;
    
            nuevoObjeto.GetComponent<CanvasGroup>().alpha = 0f;
            nuevoObjeto.GetComponent<CanvasGroup>().blocksRaycasts = false; 
            
            var arrastrable = nuevoObjeto.GetComponent<ImagenArrastrable>();
            if (arrastrable != null)
                arrastrable.enabled = false;
            
            nuevoObjeto.transform.SetParent(transform);
            
            imagenRespuesta.sprite = display.sprite;
            imagenRespuesta.enabled = true;

            OnRespuestaAsignada?.Invoke(imagenRespuesta.sprite);
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
    
            var anteriorArrastrable = _objetoActual.GetComponent<ImagenArrastrable>();
            if (anteriorArrastrable == null) return;
    
            _objetoActual.SetActive(true);
            anteriorArrastrable.RestaurarAOrigen();
            _objetoActual.GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            var arrastrable = _objetoActual.GetComponent<ImagenArrastrable>();
            if (arrastrable != null)
                arrastrable.enabled = true;
            
            imagenRespuesta.sprite = null;
            imagenRespuesta.enabled = false;
            
            _objetoActual = null;
            
            OnRespuestaLiberada?.Invoke();
        }
    }
}