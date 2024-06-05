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
    internal class JumpCancelMovementApplier : MonoBehaviour, IMovementApplier<Vector3>, IObservableMovementApplier, ICancellableMovementApplier
    {
        private IMovementPerformer<Acceleration> _accelerationMovementPerformer;
        private Task<bool> _jumpCancelTask;
        private CancellationTokenSource _jumpCancelCancellationTokenSource = new CancellationTokenSource();

        [SerializeField]
        private JumpCancelParameters _jumpCancelParameters;

        public event EventHandler MotionStarted;
        public event EventHandler MotionEnded;

        [field: SerializeField]
        public UnityEvent JumpCancelStarted { get; private set; }

        [field: SerializeField]
        public UnityEvent JumpCancelled { get; private set; }

        private bool _inMotion;
        public bool InMotion
        {
            get => _inMotion;
            set
            {
                bool wasInMotion = _inMotion;
                _inMotion = value;

                if (!wasInMotion && _inMotion)
                    JumpCancelStarted?.Invoke();
                else if (wasInMotion && !_inMotion)
                    JumpCancelled?.Invoke();
            }
        }

        private void Awake()
        {
            _jumpCancelTask = Task.FromResult(true);

            _accelerationMovementPerformer = GetComponentInChildren<IMovementPerformer<Acceleration>>();

            JumpCancelStarted.AddListener(() => MotionStarted?.Invoke(this, EventArgs.Empty));
            JumpCancelled.AddListener(() => MotionEnded?.Invoke(this, EventArgs.Empty));
        }

        private void OnDestroy()
        {
            JumpCancelStarted.RemoveAllListeners();
            JumpCancelled.RemoveAllListeners();
        }

        public Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input)
        {
            return TryStartJumpCancelTask(readOnlyRigidbody, rigidbody, input);
        }

        private Task<bool> TryStartJumpCancelTask(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input) =>
            InMotion
            ? _jumpCancelTask
            : _jumpCancelTask = JumpCancelAsync(readOnlyRigidbody, rigidbody, input, _jumpCancelCancellationTokenSource.Token);

        public void Cancel() => TryCancel();
        public void Precancel() => _jumpCancelCancellationTokenSource.Cancel();
        public bool TryCancel()
        {
            if (InMotion)
            {
                _jumpCancelCancellationTokenSource.Cancel();
                _jumpCancelCancellationTokenSource.Dispose();
                _jumpCancelCancellationTokenSource = new CancellationTokenSource();
                return true;
            }

            return false;
        }

        private async Task<bool> JumpCancelAsync(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input, CancellationToken cancellationToken)
        {
            InMotion = true;
            Acceleration jumpCancelAcceleration =
                FromCancelationAxis(readOnlyRigidbody.Velocity, input, _jumpCancelParameters.JumpCancelTime)
                - Vector3.Project(readOnlyRigidbody.GravityScale * Physics.gravity, -input);

            bool cancelled = false;
            for (float time = 0.0f; time < _jumpCancelParameters.JumpCancelTime && !cancelled; time += Time.fixedDeltaTime)
            {
                if (!_accelerationMovementPerformer.TryPerform(readOnlyRigidbody, rigidbody, jumpCancelAcceleration)
                    || cancellationToken.IsCancellationRequested)
                    cancelled = true;

                float currentFixedTime = Time.fixedTime;
                while (currentFixedTime == Time.fixedTime)
                    await Task.Yield();
            }

            InMotion = false;
            return !cancelled;
        }

        private Acceleration FromCancelationAxis(Vector3 velocity, Vector3 axis, float time) =>
            -Vector3.Project(velocity, axis) / time;
    }
}
