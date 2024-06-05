using AgnosticPhysics.Rigidbody;
using System;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Constraint
{
    internal class InMotionConstraint : MonoBehaviour, IMovementConstraint
    {
        [SerializeField]
        private GameObject _observableMovementApplierObject;
        private IObservableMovementApplier _observableMovementApplier;

        private void Awake()
        {
            _observableMovementApplier = _observableMovementApplierObject.GetComponent<IObservableMovementApplier>();
        }

        public bool IsSatisfied<TInput>(IReadOnlyRigidbody readOnlyRigidbody, TInput input) =>
            _observableMovementApplier.InMotion;
    }
}
