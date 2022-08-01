using Gameplay.Player.Actions;
using Input;
using UnityEngine;
using Zenject;

namespace UI
{
    public class Inputs : UiElement
    {
        [SerializeField] private InputStick _moveStick;
        [SerializeField] private InputPad _lookPad;
        [SerializeField] private InputPad _attackLookPad;


        [Inject]
        public void Construct(PlayerActions playerActions)
        {
            _moveStick.Setup(playerActions.PlayerMove);
            _lookPad.Setup(playerActions.PlayerLook);
            _attackLookPad.Setup(
                playerActions.PlayerLook,
                playerActions.PlayerAttack);
        }
    }
}