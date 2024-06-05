using InputBox.Writeable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ModularPlatforming.Movement.Input
{
    internal class PlayerInputtedFixedMovementWriter : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _movementActionReference;
        private IInputWriteable<Vector4> _inputWriteable;

        [SerializeField]
        private Vector4 _fixedInput;

        [SerializeField]
        private bool _continuousInput = true;

        private void Awake()
        {
            _inputWriteable = GetComponentInChildren<IInputWriteable<Vector4>>();

            if (!_continuousInput)
                _movementActionReference.action.performed += OnMovementActionPerformed;
        }

        private void OnEnable()
        {
            _movementActionReference.action.Enable();
        }

        private void OnDisable()
        {
            _movementActionReference.action.Disable();
        }

        private void OnDestroy()
        {
            _movementActionReference.action.performed -= OnMovementActionPerformed;
        }

        private void FixedUpdate()
        {
            _ = _continuousInput && _inputWriteable.TrySetInput(_fixedInput);
        }

        private void OnMovementActionPerformed(InputAction.CallbackContext context)
        {
            _inputWriteable.TrySetInput(_fixedInput);
        }
    }
}