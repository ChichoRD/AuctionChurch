using UnityEngine;

namespace AgnosticPhysics.Rigidbody
{
    public interface IRigidbody : IReadOnlyRigidbody
    {
        float IReadOnlyRigidbody.Mass => Mass;
        Vector3 IReadOnlyRigidbody.Position => Position;
        Quaternion IReadOnlyRigidbody.Rotation => Rotation;
        Vector3 IReadOnlyRigidbody.Velocity => Velocity;
        Vector3 IReadOnlyRigidbody.AngularVelocity => AngularVelocity;

        float IReadOnlyRigidbody.Drag => Drag;
        float IReadOnlyRigidbody.AngularDrag => AngularDrag;

        float IReadOnlyRigidbody.GravityScale => GravityScale;
        bool IReadOnlyRigidbody.IsKinematic => IsKinematic;
        RigidbodyConstraints IReadOnlyRigidbody.Constraints => Constraints;


        new float Mass { get; set; }
        new Vector3 Position { get; set; }
        new Quaternion Rotation { get; set; }
        new Vector3 Velocity { get; set; }
        new Vector3 AngularVelocity { get; set; }

        new float Drag { get; set; }
        new float AngularDrag { get; set; }

        new float GravityScale { get; set; }
        new bool IsKinematic { get; set; }
        new RigidbodyConstraints Constraints { get; set; }

        void AddForce(Vector3 force);
        void AddForceAtPosition(Vector3 force, Vector3 position);
        void AddRelativeForce(Vector3 force);
        void AddTorque(Vector3 torque);

        void MovePosition(Vector3 position);
        void MoveRotation(Quaternion rotation);
    }
}