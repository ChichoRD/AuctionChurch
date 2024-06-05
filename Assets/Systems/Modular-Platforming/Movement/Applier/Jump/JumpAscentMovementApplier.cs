using AgnosticPhysics.Rigidbody;
using ModularPlatforming.Movement.Applier.Jump.Parameter;
using ModularPlatforming.Movement.Magnitude;
using ModularPlatforming.Movement.Performer;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ModularPlatforming.Movement.Applier.Jump
{
    internal class JumpAscentMovementApplier : MonoBehaviour, IMovementApplier<Vector3>, IObservableMovementApplier, ICancellableMovementApplier
    {
        private IMovementPerformer<Velocity> _velocityMovementPerformer;
        private IMovementPerformer<Acceleration> _accelerationMovementPerformer;
        private Task<bool> _jumpAscentTask;
        private CancellationTokenSource _jumpAscentCancellationTokenSource = new CancellationTokenSource();

        [SerializeField]
        private JumpAscentParameters _jumpAscentParameters;

        public event EventHandler MotionStarted;
        public event EventHandler MotionEnded;

        [field: SerializeField]
        public UnityEvent JumpAscentStarted { get; private set; }

        [field: SerializeField]
        public UnityEvent JumpAscentReachedPeak { get; private set; }

        private bool _inMotion;
        public bool InMotion
        {
            get => _inMotion;
            set
            {
                bool wasInMotion = _inMotion;
                _inMotion = value;

                if (!wasInMotion && _inMotion)
                    JumpAscentStarted?.Invoke();
                else if (wasInMotion && !_inMotion)
                    JumpAscentReachedPeak?.Invoke();
            }
        }

        private void Awake()
        {
            _jumpAscentTask = Task.FromResult(true);

            _velocityMovementPerformer = GetComponentInChildren<IMovementPerformer<Velocity>>();
            _accelerationMovementPerformer = GetComponentInChildren<IMovementPerformer<Acceleration>>();

            JumpAscentStarted.AddListener(() => MotionStarted?.Invoke(this, EventArgs.Empty));
            JumpAscentReachedPeak.AddListener(() => MotionEnded?.Invoke(this, EventArgs.Empty));
        }

        private void OnDestroy()
        {
            JumpAscentStarted.RemoveAllListeners();
            JumpAscentReachedPeak.RemoveAllListeners();
        }

        public Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input)
        {
            return TryStartJumpAscentTask(readOnlyRigidbody, rigidbody, input);
        }

        private Task<bool> TryStartJumpAscentTask(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input) =>
            InMotion
            ? _jumpAscentTask
            : (_jumpAscentTask = JumpAscentAsync(readOnlyRigidbody, rigidbody, input, _jumpAscentCancellationTokenSource.Token));

        public void Cancel() => TryCancel();
        public void Precancel() => _jumpAscentCancellationTokenSource.Cancel();
        public bool TryCancel()
        {
            if (InMotion)
            {
                _jumpAscentCancellationTokenSource.Cancel();
                _jumpAscentCancellationTokenSource.Dispose();
                _jumpAscentCancellationTokenSource = new CancellationTokenSource();
                return true;
            }

            return false;
        }

        private async Task<bool> JumpAscentAsync(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input, CancellationToken cancellationToken)
        {
            InMotion = true;

            Velocity jumpVelocityIncrement = input * _jumpAscentParameters.JumpSpeed - readOnlyRigidbody.Velocity;
            if (!_velocityMovementPerformer.TryPerform(readOnlyRigidbody, rigidbody, jumpVelocityIncrement))
                return InMotion = false;

            Acceleration gravity = input * _jumpAscentParameters.JumpGravityMagnitude - readOnlyRigidbody.GravityScale * Physics.gravity;
            bool cancelled = false;
            for (float time = 0.0f; time < _jumpAscentParameters.JumpPeakReachTime && !cancelled; time += Time.fixedDeltaTime)
            {
                if (!_accelerationMovementPerformer.TryPerform(readOnlyRigidbody, rigidbody, gravity)
                    || cancellationToken.IsCancellationRequested)
                    cancelled = true;

                float currentFixedTime = Time.fixedTime;
                while (currentFixedTime == Time.fixedTime)
                    await Task.Yield();
            }

            InMotion = false;
            return !cancelled;
        }
    }
}
