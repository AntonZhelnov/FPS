using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class InputPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private IBoolSettable _boolSettable;
        private Vector2 _currentPosition;
        private Vector2 _delta;
        private bool _isPressed;
        private RectTransform _parentTransform;
        private Vector2 _previousPosition;
        private IVector2Settable _vector2Settable;


        public void Setup(
            IVector2Settable vector2Settable,
            IBoolSettable boolSettable = null)
        {
            _vector2Settable = vector2Settable;
            _boolSettable = boolSettable;

            _parentTransform = transform.parent.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_isPressed)
            {
                _delta = _currentPosition - _previousPosition;
                _previousPosition = _currentPosition;

                _vector2Settable.SetVector2(_delta);
                _boolSettable?.SetBool(_isPressed);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
                throw new ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _currentPosition);

            _boolSettable?.SetBool(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
                throw new ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _currentPosition);
            _previousPosition = _currentPosition;

            _boolSettable?.SetBool(true);
            _isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _currentPosition = Vector2.zero;
            _previousPosition = Vector2.zero;
            _delta = Vector2.zero;

            _boolSettable?.SetBool(false);
            _vector2Settable.SetVector2(Vector2.zero);
            _isPressed = false;
        }
    }
}