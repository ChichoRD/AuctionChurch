using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Magnitude;
using UnityEngine;

namespace ModularPlatforming.Movement.Performer
{
    internal class AsymptoticMovementPerformer : MonoBehaviour, IMovementPerformer<Velocity>
    {
        [SerializeField]
        [Min(0.0f)]
        private float _speedReachTime = 2.0f;

        public bool TryPerform(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Velocity magnitude)
        {
            Vector3 targetVelocity = magnitude;
            Vector3 currentVelocity = readOnlyRigidbody.Velocity;

            float velocityRatio = Vector3.Dot(targetVelocity, currentVelocity) / (Vector3.Dot(currentVelocity, currentVelocity) + 1.0f);
            float velocityIncrementFactor = (Mathf.Log(Mathf.Abs(velocityRatio) + 1.0f) + 1.0f) / (_speedReachTime + 1.0f);

            Vector3 velocityIncrement = (targetVelocity - currentVelocity) * velocityIncrementFactor;
            rigidbody.AddVelocity(velocityIncrement);
            return true;
        }
    }
}