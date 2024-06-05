using AgnosticPhysics.Rigidbody;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier
{
    internal class MovementApplier : MonoBehaviour, IMovementApplier
    {
        public Task<bool> TryApply<TInput>(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, TInput input, IMovementApplier<TInput> movementApplier) =>
            movementApplier.TryApply(readOnlyRigidbody, rigidbody, input);
    }
}
