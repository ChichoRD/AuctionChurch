using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Applier.Jump.Parameter;
using System;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Constraint
{
    internal class MinJumpHeightConstraint : MonoBehaviour, IMovementConstraint
    {
        [SerializeField]
        private GameObject _observableMovementApplierObject;
        private IObservableMovementApplier _observableMovementApplier;

        [SerializeField]
        private JumpAscentParameters _jumpAscentParameters;
        private DateTime _motionStartedDateTime;

        private void Awake()
        {
            _observableMovementApplier = _observableMovementApplierObject.GetComponent<IObservableMovementApplier>();
            _observableMovementApplier.MotionStarted += OnMotionStarted;
        }

        private void OnDestroy()
        {
            _observableMovementApplier.MotionStarted -= OnMotionStarted;
        }

        private void OnMotionStarted(object sender, EventArgs e) => _motionStartedDateTime = DateTime.Now; 

        public bool IsSatisfiedWithWindow(float window) =>
            (DateTime.Now - _motionStartedDateTime).TotalSeconds
            >= TimeSpan.FromSeconds(_jumpAscentParameters.MinJumpHeightReachTime).TotalSeconds - window;
        public bool IsSatisfied<TInput>(IReadOnlyRigidbody readOnlyRigidbody, TInput input) =>
            (DateTime.Now - _motionStartedDateTime).TotalSeconds
            >= TimeSpan.FromSeconds(_jumpAscentParameters.MinJumpHeightReachTime).TotalSeconds;
    }
}
