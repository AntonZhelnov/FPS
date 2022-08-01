using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TmpFormattedIntSetter : MonoBehaviour, IInitializable
    {
        private string _format;
        private int _initialValue;
        private TextMeshProUGUI _textMeshPro;


        [Inject]
        public void Construct(
            string format,
            int value)
        {
            _format = format;
            _initialValue = value;
        }

        [Inject]
        public void Initialize()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            SetValue(_initialValue);
        }

        public void SetValue(int value)
        {
            _textMeshPro.SetText(_format, value);
        }
    }
}