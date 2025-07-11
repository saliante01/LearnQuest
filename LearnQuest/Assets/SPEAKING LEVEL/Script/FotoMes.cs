using UnityEngine;

namespace Assets.SPEAKING_LEVEL.Script
{
    [CreateAssetMenu(fileName = "FotoMes", menuName = "Recursos/FotoMes")]
    public class FotoMes : ScriptableObject
    {
        public Sprite Imagen;
        public string NombreMes;
    }
}