using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class InputStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private float _movementRange = 50f;

        private RectTransform _parentTransform;
        private Vector2 _pointerDownPosition;
        private Vector2 _startPosition;
        private IVector2Settable _vector2Settable;


        public void Setup(IVector2Settable vector2Settable)
        {
            _vector2Settable = vector2Settable;

            _parentTransform = transform.parent.GetComponent<RectTransform>();
            _startPosition = ((RectTransform)transform).anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
                throw new ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var position);

            var delta = position - _pointerDownPosition;
            delta = Vector2.ClampMagnitude(delta, _movementRange);

            ((RectTransform)transform).anchoredPosition = _startPosition + delta;

            _vector2Settable.SetVector2(
                new Vector2(
                    delta.x / _movementRange,
                    delta.y / _movementRange));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
                throw new ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _pointerDownPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ((RectTransform)transform).anchoredPosition = _startPosition;
            _vector2Settable.SetVector2(Vector2.zero);
        }
    }
}