using UnityEngine;

namespace Common.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public abstract class UIItem : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;


        public virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        protected virtual void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}