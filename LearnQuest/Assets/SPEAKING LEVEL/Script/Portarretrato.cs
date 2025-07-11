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
        
        public bool ReconocidoPorVoz { get; private set; } = false;
        
        private void Start()
        {
            _imagen = GetComponent<Image>();
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

            _imagen.sprite = fotoMesDisplay.FotoMes.Imagen;
            FotoMesActual = fotoMesDisplay.FotoMes;

            _objetoActual = nuevoObjeto;

            nuevoObjeto.transform.SetParent(transform);
            nuevoObjeto.SetActive(false);
        }
        
        public void CambiarColorVerdoso()
        {
            if (_imagen != null)
                _imagen.color = new Color(0.6f, 1f, 0.6f, 1f); 
        }
    }
}