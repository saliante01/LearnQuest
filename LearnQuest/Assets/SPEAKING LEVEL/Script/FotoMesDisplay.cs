using UnityEngine;
using UnityEngine.UI;

namespace Assets.SPEAKING_LEVEL.Script
{
    public class FotoMesDisplay : MonoBehaviour
    {
        public FotoMes FotoMes;
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            if (_image == null)
            {
                Debug.LogWarning($"{nameof(FotoMesDisplay)} requiere un componente Image en el mismo GameObject.");
                return;
            }

            if (FotoMes != null)
                _image.sprite = FotoMes.Imagen;
        }
    }
}