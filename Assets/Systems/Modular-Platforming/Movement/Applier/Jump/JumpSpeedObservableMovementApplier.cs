using AgnosticPhysics.Rigidbody;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ModularPlatforming.Movement.Applier
{
    internal class JumpSpeedObservableMovementApplier : MonoBehaviour, IMovementApplier<Vector3>
    {
        private IMovementApplier<Vector3> _movementApplier;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        [field: SerializeField]
        public UnityEvent JumpSpeedBecamePositive { get; private set; }

        [field: SerializeField]
        public UnityEvent JumpSpeedBecameNegative { get; private set; }

        private float _currentJumpSpeed;

        public float CurrentJumpSpeed
        {
            get => _currentJumpSpeed;
            set 
            {
                float previousJumpSpeed = _currentJumpSpeed;
                _currentJumpSpeed = value;

                if (previousJumpSpeed < 0 && _currentJumpSpeed >= 0)
                    JumpSpeedBecamePositive.Invoke();
                else if (previousJumpSpeed > 0 && _currentJumpSpeed <= 0)
                    JumpSpeedBecameNegative.Invoke();
            }
        }

        public bool InMotion { get; private set; }

        private void Awake()
        {
            _movementApplier = GetComponentsInChildren<IMovementApplier<Vector3>>().FirstOrDefault(m => m != (IMovementApplier<Vector3>)this);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Dispose();
        }

        public async Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _ = UpdateAlignedSpeedAsync(readOnlyRigidbody, input, _cancellationTokenSource.Token);

            InMotion = true;
            bool success = await _movementApplier.TryApply(readOnlyRigidbody, rigidbody, input);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            InMotion = false;
            return success;
        }

        private async Task UpdateAlignedSpeedAsync(IReadOnlyRigidbody readOnlyRigidbody, Vector3 input, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                CurrentJumpSpeed = Vector3.Dot(readOnlyRigidbody.Velocity, input);

                float currentFixedTime = Time.fixedTime;
                while (currentFixedTime == Time.fixedTime)
                    await Task.Yield();
            }
        }
    }
}
