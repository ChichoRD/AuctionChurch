using AgnosticPhysics.Rigidbody;
using InputBox.Readable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ModularPlatforming.Movement.Performer.Test
{
    internal class MovementPerformerTest : MonoBehaviour, IInputReadable<Vector2>
    {
        [SerializeField]
        private InputActionReference _movementActionReference;

        private IRigidbody _rigidbody;
        private IMovementPerformer<Vector2> _movementPerformer;

        private void Awake()
        {
            _rigidbody = GetComponent<IRigidbody>();
            _movementPerformer = GetComponent<IMovementPerformer<Vector2>>();
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
            _movementPerformer.TryPerform(_rigidbody, _rigidbody, GetInput());
        }

        public Vector2 GetInput() => _movementActionReference.action.ReadValue<Vector2>();
    }
}