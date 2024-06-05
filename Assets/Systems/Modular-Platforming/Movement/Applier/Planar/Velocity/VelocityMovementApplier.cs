using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Performer;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Planar.Velocity
{
    internal class VelocityMovementApplier : MonoBehaviour, IMovementApplier<Vector3>
    {
        private IMovementPerformer<Magnitude.Velocity> _movementPerformer;

        [SerializeField]
        private float _speed = 5.0f;

        private void Awake()
        {
            _movementPerformer = GetComponentInChildren<IMovementPerformer<Magnitude.Velocity>>();
        }

        public Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input) =>
            Task.FromResult(_movementPerformer.TryPerform(readOnlyRigidbody, rigidbody, input * _speed));

    }
}
