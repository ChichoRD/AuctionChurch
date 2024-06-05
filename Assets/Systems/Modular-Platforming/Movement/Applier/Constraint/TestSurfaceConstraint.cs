using AgnosticPhysics.Rigidbody;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Constraint
{
    internal class TestSurfaceConstraint : MonoBehaviour, IMovementConstraint
    {
        [SerializeField]
        private Transform _originTrasnform;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private Vector3 _surfaceComparerNormal = Vector3.up;

        [SerializeField]
        private float _minAngle;

        [SerializeField]
        private float _maxAngle;

        public bool IsSatisfied<TInput>(IReadOnlyRigidbody readOnlyRigidbody, TInput input)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(readOnlyRigidbody.Position, Vector2.down, 1.0f, _layerMask);
            return raycastHit.collider != null && IsAngleBetween(raycastHit.normal, _minAngle, _maxAngle);
        }

        private bool IsAngleBetween(Vector3 normal, float minAngle, float maxAngle)
        {
            float angle = Vector3.Angle(normal, _surfaceComparerNormal);
            return angle >= minAngle && angle <= maxAngle;
        }
    }
}