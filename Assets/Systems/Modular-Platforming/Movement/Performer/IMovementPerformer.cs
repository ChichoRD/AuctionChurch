using AgnosticPhysics.Rigidbody;

namespace ModularPlatforming.Movement.Performer
{
    public interface IMovementPerformer<in TMagnitude>
    {
        bool TryPerform(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, TMagnitude magnitude);
    }
}