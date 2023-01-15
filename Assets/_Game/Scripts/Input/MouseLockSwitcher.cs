using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class MouseLockSwitcher : IInitializable, IDisposable
    {
        private readonly Controls _controls;


        public MouseLockSwitcher(Controls controls)
        {
            _controls = controls;
        }

        public void Initialize()
        {
            _controls.Player.SwitchLock.performed += SwitchLock;
        }

        public void Dispose()
        {
            _controls.Player.SwitchLock.performed -= SwitchLock;
        }

        private void SwitchLock(InputAction.CallbackContext _)
        {
            switch (Cursor.lockState)
            {
                case CursorLockMode.Locked:
                {
                    Cursor.lockState = CursorLockMode.None;
                    _controls.Player.Look.Disable();
                    _controls.Player.Attack.Disable();
                    break;
                }

                case CursorLockMode.None:
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    _controls.Player.Look.Enable();
                    _controls.Player.Attack.Enable();
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}