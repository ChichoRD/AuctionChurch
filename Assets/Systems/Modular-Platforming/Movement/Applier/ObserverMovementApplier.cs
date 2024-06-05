using AgnosticPhysics.Rigidbody;
using InputBox.Observable;
using InputBox.Readable;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier
{
    internal class ObserverMovementApplier : MonoBehaviour
    {
        [SerializeField]
        private Object _rigidbodyObject;
        private IRigidbody _rigidbody;
        private IMovementApplier _applier;
        private IMovementApplier<Vector3> _movementApplier;
        private IObservableInput _observableInput;
        private IInputReadable<Vector3> _inputReadable;

        private void Awake()
        {
            _rigidbody = _rigidbodyObject as IRigidbody ?? GetComponent<IRigidbody>();
            _applier = GetComponentInChildren<IMovementApplier>();
            _movementApplier = GetComponentInChildren<IMovementApplier<Vector3>>();
            _observableInput = GetComponentInChildren<IObservableInput>();
            _inputReadable = GetComponentInChildren<IInputReadable<Vector3>>();

            _observableInput.InputReceived.AddListener(OnInputReceived);
        }

        private void OnDestroy()
        {
            _observableInput.InputReceived.RemoveListener(OnInputReceived);
        }

        private void OnInputReceived()
        {
            _applier.TryApply(_rigidbody, _rigidbody, _inputReadable.GetInput(), _movementApplier);
        }
    }
}
