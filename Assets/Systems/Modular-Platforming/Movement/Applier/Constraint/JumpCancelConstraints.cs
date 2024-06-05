using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Applier.Jump.Parameter;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Constraint
{
    internal class JumpCancelConstraints : MonoBehaviour, IMovementConstraint
    {
        private IMovementConstraint _inMotionConstraint;
        private MinJumpHeightConstraint _minJumpHeightConstraint;
        [SerializeField]
        private JumpCancelParameters _jumpCancelParameters;

        private void Awake()
        {
            _inMotionConstraint = GetComponentInChildren<InMotionConstraint>();
            _minJumpHeightConstraint = GetComponentInChildren<MinJumpHeightConstraint>();
        }

        public bool IsSatisfied<TInput>(IReadOnlyRigidbody readOnlyRigidbody, TInput input) =>
            _inMotionConstraint.IsSatisfied(readOnlyRigidbody, input)
            && _minJumpHeightConstraint.IsSatisfiedWithWindow(_jumpCancelParameters?.JumpCancelTime ?? 0.0f);
    }
}
