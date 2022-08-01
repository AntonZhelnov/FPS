using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class UiElement : MonoBehaviour, IInitializable
    {
        [Inject]
        public virtual void Initialize()
        {
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}