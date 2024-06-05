using AgnosticPhysics.Rigidbody;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier
{
    internal class ConstrainedRetryMovementApplier : MonoBehaviour, IMovementApplier
    {
        private IMovementApplier _movementApplier;
        [SerializeField]
        private GameObject _observableMovementApplierObject;
        private IObservableMovementApplier _observableMovementApplier;

        [SerializeField]
        [Min(0)]
        private int _retryCount = 0;
        private int _remainingTries;

        [SerializeField]
        private bool _unlimitedRetries = true;

        private void Awake()
        {
            _movementApplier = GetComponentsInChildren<IMovementApplier>().FirstOrDefault(c => c != (IMovementApplier)this);
            _observableMovementApplier = _observableMovementApplierObject.GetComponent<IObservableMovementApplier>();
        }

        public async Task<bool> TryApply<TInput>(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, TInput input, IMovementApplier<TInput> movementApplier)
        {
            _remainingTries = _retryCount + 1;
            while ((_unlimitedRetries || _remainingTries-- > 0)
                   && _observableMovementApplier.InMotion
                   && !await _movementApplier.TryApply(readOnlyRigidbody, rigidbody, input, movementApplier))
            {
                float currentFixedTime = Time.fixedTime;
                while (currentFixedTime == Time.fixedTime)
                    await Task.Yield();
            }

            return _observableMovementApplier.InMotion && (_unlimitedRetries || _remainingTries >= 0);
        }
    }
}
