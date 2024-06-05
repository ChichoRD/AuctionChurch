using UnityEngine;

namespace AgnosticPhysics.Rigidbody
{
    public interface IReadOnlyRigidbody
    {
        GameObject GameObject { get; }

        float Mass { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Velocity { get; }
        Vector3 AngularVelocity { get; }

        float Drag { get; }
        float AngularDrag { get; }

        float GravityScale { get; }
        bool IsKinematic { get; }
        RigidbodyConstraints Constraints { get; }
    }
}