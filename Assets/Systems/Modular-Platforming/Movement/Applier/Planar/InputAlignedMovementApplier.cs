using AgnosticPhysics.Rigidbody;
using InputBox.Readable;
using ModularPlatforming.Movement.Measure;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Planar
{
    internal class InputAlignedMovementApplier : MonoBehaviour, IMovementApplier<Vector3>
    {
        [SerializeField]
        private Object _velocityMovementApplierObject;
        private IMovementApplier<Vector3> _velocityMovementApplier;

        [SerializeField]
        private Object _inputReadableObject;
        private IInputReadable<Vector3> _inputReadable;

        private void Awake()
        {
            _velocityMovementApplier = _velocityMovementApplierObject as IMovementApplier<Vector3>;
            _inputReadable = _inputReadableObject as IInputReadable<Vector3>;
        }

        public Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input) =>
            _velocityMovementApplier.TryApply(
                new InputAlignedRigidbodyMeasurer(readOnlyRigidbody, _inputReadable),
                rigidbody,
                input);

    }
}
