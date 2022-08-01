using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(
        fileName = "New Animation Config",
        menuName = "Configs/Animation Config")]
    public class AnimationConfig : ScriptableObject
    {
        [SerializeField] private string _triggerName;
        [SerializeField] [Min(0)] private float _actionDelay;

        public string TriggerName => _triggerName;
        public float ActionDelay => _actionDelay;
    }
}