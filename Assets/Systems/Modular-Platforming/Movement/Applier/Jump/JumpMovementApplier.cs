using AgnosticPhysics.Rigidbody;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Jump
{
    internal class JumpMovementApplier : MonoBehaviour, IMovementApplier<Vector3>
    {
        private IMovementApplier<Vector3> _jumpAscentMovementApplier;
        private IMovementApplier<Vector3> _jumpDescentMovementApplier;
        private ICancellableMovementApplier _jumpDescentCancellableMovementApplier;

        private void Awake()
        {
            _jumpAscentMovementApplier = GetComponentInChildren<JumpAscentMovementApplier>();

            JumpDescentMovementApplier jumpDescentMovementApplier = GetComponentInChildren<JumpDescentMovementApplier>();
            _jumpDescentMovementApplier = jumpDescentMovementApplier;
            _jumpDescentCancellableMovementApplier = jumpDescentMovementApplier;
        }

        public Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input)
        {
            return JumpAsync(readOnlyRigidbody, rigidbody, input);
        }

        private async Task<bool> JumpAsync(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input)
        {
            _jumpDescentCancellableMovementApplier.TryCancel();
            return await _jumpAscentMovementApplier.TryApply(readOnlyRigidbody, rigidbody, input)
                & await _jumpDescentMovementApplier.TryApply(readOnlyRigidbody, rigidbody, input);
        }
    }
}
