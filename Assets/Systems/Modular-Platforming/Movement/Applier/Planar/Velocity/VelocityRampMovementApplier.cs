using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Performer;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Planar.Velocity
{
    internal class VelocityRampMovementApplier : MonoBehaviour, IMovementApplier<Vector3>, ICancellableMovementApplier
    {
        private IMovementPerformer<Magnitude.Velocity> _movementPerformer;
        private float _currentSpeedFactor;
        private Vector3 _lastValidInput;

        private bool _inAcceleration;
        private bool _inDeceleration;
        private CancellationTokenSource _accelerationCancellationTokenSource = new CancellationTokenSource();
        private CancellationTokenSource _decelerationCancellationTokenSource = new CancellationTokenSource();

        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        [Min(0.0f)]
        private float _accelerationTime = 0.5f;
        [SerializeField]
        private AnimationCurve _accelerationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        [SerializeField]
        [Min(0.0f)]
        private float _decelerationTime = 0.5f;
        [SerializeField]
        private AnimationCurve _decelerationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        private void Awake()
        {
            _movementPerformer = GetComponentInChildren<IMovementPerformer<Magnitude.Velocity>>();
        }

        public Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input)
        {
            _ = InitiateVelocityRamp(input);
            return Task.FromResult(_movementPerformer.TryPerform(readOnlyRigidbody, rigidbody, _currentSpeedFactor * _speed * _lastValidInput));

            async Task<bool> InitiateVelocityRamp(Vector3 input)
            {
                return (TryStoreLastValidInput(input)
                       ? !(_inAcceleration && TryStopVelocityRampTask(ref _decelerationCancellationTokenSource))
                         | !_inAcceleration && await AccelerationRampAsync(1.0f, _accelerationTime, _accelerationCurve, _accelerationCancellationTokenSource.Token)
                       : !(_inDeceleration && TryStopVelocityRampTask(ref _accelerationCancellationTokenSource))
                         | !_inDeceleration && await DecelerationRampAsync(0.0f, _decelerationTime, _decelerationCurve, _decelerationCancellationTokenSource.Token));
            }
        }

        private bool TryStoreLastValidInput(Vector3 input)
        {
            if (Mathf.Approximately(input.sqrMagnitude, 0.0f))
                return false;

            _lastValidInput = input;
            return true;
        }

        private bool TryStopVelocityRampTask(ref CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();

            return false;
        }

        public bool TryCancel() =>
            (_inAcceleration && TryStopVelocityRampTask(ref _accelerationCancellationTokenSource))
            | (_inDeceleration && TryStopVelocityRampTask(ref _decelerationCancellationTokenSource));

        private async Task<bool> AccelerationRampAsync(float targetSpeedFactor, float duration, AnimationCurve curve, CancellationToken cancellationToken)
        {
            _inAcceleration = true;
            bool success = await VelocityRampAsync(targetSpeedFactor, duration, curve, cancellationToken);
            _inAcceleration = false;
            return success;
        }

        private async Task<bool> DecelerationRampAsync(float targetSpeedFactor, float duration, AnimationCurve curve, CancellationToken cancellationToken)
        {
            _inDeceleration = true;
            bool success = await VelocityRampAsync(targetSpeedFactor, duration, curve, cancellationToken);
            _inDeceleration = false;
            return success;
        }

        private async Task<bool> VelocityRampAsync(float targetSpeedFactor, float duration, AnimationCurve curve, CancellationToken cancellationToken)
        {
            for (float t = 0.0f; t < duration && !cancellationToken.IsCancellationRequested; t += Time.fixedDeltaTime)
            {
                float normalizedTime = t / duration;
                _currentSpeedFactor = Mathf.Lerp(_currentSpeedFactor, targetSpeedFactor, curve.Evaluate(normalizedTime));

                float currentFixedTime = Time.fixedTime;
                while (currentFixedTime == Time.fixedTime)
                    await Task.Yield();
            }

            _currentSpeedFactor = targetSpeedFactor;
            return true;
        }
    }
}
