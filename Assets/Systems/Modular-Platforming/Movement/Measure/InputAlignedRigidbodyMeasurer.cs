using AgnosticPhysics.Rigidbody;
using InputBox.Readable;
using UnityEngine;

namespace ModularPlatforming.Movement.Measure
{
    internal readonly struct InputAlignedRigidbodyMeasurer : IReadOnlyRigidbody
    {
        private readonly IReadOnlyRigidbody _rigidbody;
        private readonly IInputReadable<Vector3> _inputReadable;

        public InputAlignedRigidbodyMeasurer(IReadOnlyRigidbody rigidbody, IInputReadable<Vector3> inputReadable)
        {
            _rigidbody = rigidbody;
            _inputReadable = inputReadable;
        }

        public GameObject GameObject => _rigidbody.GameObject;

        public float Mass => _rigidbody.Mass;

        public Vector3 Position => _rigidbody.Position;

        public Quaternion Rotation => _rigidbody.Rotation;

        public Vector3 Velocity => _inputReadable.GetInput() * Vector3.Dot(_rigidbody.Velocity, _inputReadable.GetInput());

        public Vector3 AngularVelocity => _rigidbody.AngularVelocity;

        public float Drag => _rigidbody.Drag;

        public float AngularDrag => _rigidbody.AngularDrag;

        public float GravityScale => _rigidbody.GravityScale;

        public bool IsKinematic => _rigidbody.IsKinematic;

        public RigidbodyConstraints Constraints => _rigidbody.Constraints;
    }
}