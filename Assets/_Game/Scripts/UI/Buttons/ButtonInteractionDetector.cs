using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonInteractionDetector : UiElement
    {
        public event Action Interacted;

        private Button _button;


        public override void Initialize()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnInteracted);
        }

        private void OnInteracted()
        {
            Interacted?.Invoke();
        }
    }
}