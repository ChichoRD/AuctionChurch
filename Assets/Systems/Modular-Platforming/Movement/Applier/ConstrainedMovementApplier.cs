using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Applier.Constraint;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier
{
    internal class ConstrainedMovementApplier : MonoBehaviour, IMovementApplier, IMovementConstraint
    {
        private IMovementConstraint _movementConstraint;

        private void Awake()
        {
            _movementConstraint = GetComponentsInChildren<IMovementConstraint>().FirstOrDefault(c => c != (IMovementConstraint)this);
        }

        public Task<bool> TryApply<TInput>(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, TInput input, IMovementApplier<TInput> movementApplier) =>
            _movementConstraint.IsSatisfied(readOnlyRigidbody, input) ? movementApplier.TryApply(readOnlyRigidbody, rigidbody, input) : Task.FromResult(false);

        public bool IsSatisfied<TInput>(IReadOnlyRigidbody readOnlyRigidbody, TInput input) =>
            _movementConstraint.IsSatisfied(readOnlyRigidbody, input);
    }
}
