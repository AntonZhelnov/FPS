using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Common.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class TmpFormattedIntSetter : MonoBehaviour
    {
        private Settings _settings;
        private TextMeshProUGUI _textMeshPro;


        [Inject]
        public void Construct(Settings settings)
        {
            _settings = settings;

            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            SetValue(0);
        }

        public void SetValue(int value)
        {
            _textMeshPro.SetText(_settings.Format, value);
        }

        [Serializable]
        public class Settings
        {
            public string Format = "Value: {0}";
        }
    }
}