using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Magnitude;
using UnityEngine;

namespace ModularPlatforming.Movement.Performer
{
    internal class InstantMovementPerformer : MonoBehaviour,
        IMovementPerformer<Velocity>, IMovementPerformer<Acceleration>
    {
        public bool TryPerform(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Velocity magnitude)
        {
            rigidbody.AddVelocity(magnitude);
            return true;
        }

        public bool TryPerform(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Acceleration magnitude)
        {
            rigidbody.AddAcceleration(magnitude);
            return true;
        }
    }
}