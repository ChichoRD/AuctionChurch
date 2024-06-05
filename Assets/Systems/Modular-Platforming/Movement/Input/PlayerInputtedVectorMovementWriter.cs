using InputBox.Writeable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ModularPlatforming.Movement.Input
{
    internal class PlayerInputtedVectorMovementWriter : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _movementActionReference;
        private IInputWriteable<Vector3> _inputWriteable;

        [SerializeField]
        private bool _continuousInput = true;

        private void Awake()
        {
            _inputWriteable = GetComponentInChildren<IInputWriteable<Vector3>>();

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

        private void FixedUpdate()
        {
            _ = _continuousInput && _inputWriteable.TrySetInput(_movementActionReference.action.ReadValue<Vector2>());
        }

        private void OnDestroy()
        {
            _movementActionReference.action.performed -= OnMovementActionPerformed;
        }

        private void OnMovementActionPerformed(InputAction.CallbackContext context)
        {
            _inputWriteable.TrySetInput(context.ReadValue<Vector2>());
        }
    }
}