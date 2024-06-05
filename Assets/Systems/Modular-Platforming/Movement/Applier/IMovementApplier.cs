using AgnosticPhysics.Rigidbody;
using System.Threading.Tasks;

namespace ModularPlatforming.Movement.Applier
{
    public interface IMovementApplier<in TInput>
    {
        Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, TInput input);
    }

    public interface IMovementApplier
    {
        Task<bool> TryApply<TInput>(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, TInput input, IMovementApplier<TInput> movementApplier);
    }
}
