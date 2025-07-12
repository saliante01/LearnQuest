using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.SPEAKING_LEVEL.Script
{
    public class ImagenArrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector2 _originalPosition;
        private Transform _originalParent;
        public event Action<GameObject> OnComenzarArrastrar;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1;
            _originalPosition = _rectTransform.anchoredPosition;
            _originalParent = transform.parent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false; 
            transform.SetParent(transform.root); 
            OnComenzarArrastrar?.Invoke(gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / GetCanvasScaleFactor();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            if (transform.parent != transform.root) return;
            RestaurarAOrigen();
        }
        
        public void RestaurarAOrigen()
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            transform.SetParent(_originalParent);
            _rectTransform.anchoredPosition = _originalPosition;
        }

        private float GetCanvasScaleFactor()
        {
            var canvas = GetComponentInParent<Canvas>();
            return canvas ? canvas.scaleFactor : 1f;
        }
    }
}