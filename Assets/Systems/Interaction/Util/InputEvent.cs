using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace AuctionChurch.UtilComponents.Input
{
    public class InputEvent : MonoBehaviour
    {
        private enum InputPhase { Started, Performed, Canceled };

        [Header("Input")]
        [SerializeField] private InputActionReference _inputActionReference;
        [SerializeField] private InputPhase _inputPhase;

        [Header("Events")]
        [SerializeField] private UnityEvent _inputEvent;

        private void OnEnable()
        {
            _inputActionReference.action.Enable();
            _inputActionReference.action.started += OnInput;
            _inputActionReference.action.performed += OnInput;
            _inputActionReference.action.canceled += OnInput;
        }

        private void OnDisable()
        {
            _inputActionReference.action.Disable();
            _inputActionReference.action.started -= OnInput;
            _inputActionReference.action.performed -= OnInput;
            _inputActionReference.action.canceled -= OnInput;
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            switch (_inputPhase)
            {
                case InputPhase.Started:
                    if (ctx.started) _inputEvent.Invoke();
                    break;
                case InputPhase.Performed:
                    if (ctx.performed) _inputEvent.Invoke();
                    break;
                case InputPhase.Canceled:
                    if (ctx.canceled) _inputEvent.Invoke();
                    break;
            }
        }
    }
}