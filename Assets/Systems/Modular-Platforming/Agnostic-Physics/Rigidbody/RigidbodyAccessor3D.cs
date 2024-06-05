using UnityEngine;

namespace AgnosticPhysics.Rigidbody
{
    public class RigidbodyAccessor3D : MonoBehaviour, IRigidbody
    {
        [SerializeField] private UnityEngine.Rigidbody _rigidbody;

        public GameObject GameObject => gameObject;

        public float Mass
        {
            get => _rigidbody.mass;
            set => _rigidbody.mass = value;
        }

        public Vector3 Position
        {
            get => _rigidbody.position;
            set => _rigidbody.position = value;
        }

        public Quaternion Rotation
        {
            get => _rigidbody.rotation;
            set => _rigidbody.rotation = value;
        }

        public Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public Vector3 AngularVelocity
        {
            get => _rigidbody.angularVelocity;
            set => _rigidbody.angularVelocity = value;
        }

        public float Drag
        {
            get => _rigidbody.drag;
            set => _rigidbody.drag = value;
        }

        public float AngularDrag
        {
            get => _rigidbody.angularDrag;
            set => _rigidbody.angularDrag = value;
        }

        public float GravityScale
        {
            get => _rigidbody.useGravity ? 1.0f : 0.0f;
            set => _rigidbody.useGravity = value != 0.0f;
        }

        public bool IsKinematic
        {
            get => _rigidbody.isKinematic;
            set => _rigidbody.isKinematic = value;
        }

        public RigidbodyConstraints Constraints
        {
            get => _rigidbody.constraints;
            set => _rigidbody.constraints = value;
        }

        public void AddForce(Vector3 force)
        {
            _rigidbody.AddForce(force);
        }

        public void AddForceAtPosition(Vector3 force, Vector3 position)
        {
            _rigidbody.AddForceAtPosition(force, position);
        }

        public void AddRelativeForce(Vector3 force)
        {
            _rigidbody.AddRelativeForce(force);
        }

        public void AddTorque(Vector3 torque)
        {
            _rigidbody.AddTorque(torque);
        }

        public void MovePosition(Vector3 position)
        {
            _rigidbody.MovePosition(position);
        }

        public void MoveRotation(Quaternion rotation)
        {
            _rigidbody.MoveRotation(rotation);
        }
    }
}