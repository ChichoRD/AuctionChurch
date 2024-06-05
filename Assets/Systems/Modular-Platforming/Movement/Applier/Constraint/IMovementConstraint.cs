using AgnosticPhysics.Rigidbody;

namespace ModularPlatforming.Movement.Applier.Constraint
{
    public interface IMovementConstraint
    {
        bool IsSatisfied<TInput>(IReadOnlyRigidbody readOnlyRigidbody, TInput input);
    }
}