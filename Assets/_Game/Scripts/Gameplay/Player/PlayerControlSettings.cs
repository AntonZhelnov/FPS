using System;
using UnityEngine;

namespace Gameplay.Player
{
    [Serializable]
    public class PlayerControlSettings
    {
        [Min(0.1f)] public float MovementSpeed = 1f;
        [Min(0.1f)] public float LookSensitivity = 10f;
        [Min(0f)] public float CameraPitchLimit = 45f;
    }
}