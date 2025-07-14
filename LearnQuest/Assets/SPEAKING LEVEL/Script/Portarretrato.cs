using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.SPEAKING_LEVEL.Script

{
    public class Portarretrato : MonoBehaviour, IDropHandler
    {
        public delegate void FotoMesAsignadaHandler();
        public event FotoMesAsignadaHandler OnFotoMesAsignada;
        
        private GameObject _objetoActual;
        private Image _imagen;
        public FotoMes FotoMesActual { get; private set; }
        
        public string NombreMesActual => FotoMesActual != null ? FotoMesActual.NombreMes : "";
        
        public bool ReconocidoPorVoz { get; set; } = false;
        
        private Sprite _imagenInicial;
        
        private void Start()
        {
            _imagen = GetComponent<Image>();
            _imagenInicial = _imagen.sprite;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var objetoArrastrado = eventData.pointerDrag;
            if (!EsObjetoArrastrable(objetoArrastrado)) return;

            ReemplazarObjetoActualSiSePuede();

            AsignarNuevaImagen(objetoArrastrado);

            OnFotoMesAsignada?.Invoke();

            Debug.Log($"Imagen colocada en el portarretrato: {NombreMesActual}");
        }
        
        private bool EsObjetoArrastrable(GameObject objeto)
        {
            if (objeto == null) return false;

            var fotoMesDisplay = objeto.GetComponent<FotoMesDisplay>();
            return fotoMesDisplay != null && fotoMesDisplay.FotoMes != null;
        }
        
        private void ReemplazarObjetoActualSiSePuede()
        {
            if (_objetoActual == null) return;
            
            var anteriorArrastrable = _objetoActual.GetComponent<ImagenArrastrable>();
            
            if (anteriorArrastrable == null) return;
            
            _objetoActual.SetActive(true);
            anteriorArrastrable.RestaurarAOrigen();
        }

        private void AsignarNuevaImagen(GameObject nuevoObjeto)
        {
            var fotoMesDisplay = nuevoObjeto.GetComponent<FotoMesDisplay>();

            FotoMesActual = fotoMesDisplay.FotoMes;
            _objetoActual = nuevoObjeto;

            _imagen.sprite = fotoMesDisplay.FotoMes.Imagen;

            var arrastrable = nuevoObjeto.GetComponent<ImagenArrastrable>();
            
            if (arrastrable != null)
            {
                arrastrable.OnComenzarArrastrar -= LiberarSiContiene;
                arrastrable.OnComenzarArrastrar += LiberarSiContiene;
            }
            
            nuevoObjeto.GetComponent<CanvasGroup>().alpha = 0f;
            nuevoObjeto.transform.SetParent(transform);
        }
        
        private void LiberarSiContiene(GameObject objeto)
        {
            if (_objetoActual != objeto) return;
            
            _objetoActual.GetComponent<CanvasGroup>().alpha = 1f;
            _objetoActual = null;
            FotoMesActual = null;
            _imagen.sprite = _imagenInicial;
            OnFotoMesAsignada?.Invoke();
            
            Debug.Log($"Portarretrato liberó la imagen: {NombreMesActual}");
        }
        
        public void CambiarColorVerdoso()
        {
            if (_imagen != null)
                _imagen.color = new Color(0.6f, 1f, 0.6f, 1f); 
        }

        public void DesabilitarArrastre()
        {
            if (_objetoActual == null) return;

            var arrastrable = _objetoActual.GetComponent<ImagenArrastrable>();
            if (arrastrable != null)
                arrastrable.enabled = false;

            var canvasGroup = _objetoActual.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
                canvasGroup.blocksRaycasts = false; 
        }
    }
}