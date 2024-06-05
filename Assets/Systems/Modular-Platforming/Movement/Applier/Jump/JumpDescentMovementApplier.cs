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
    internal class JumpDescentMovementApplier : MonoBehaviour, IMovementApplier<Vector3>, IObservableMovementApplier, ICancellableMovementApplier
    {
        private IMovementPerformer<Acceleration> _accelerationMovementPerformer;
        private Task<bool> _jumpDescentTask;
        private CancellationTokenSource _jumpDescentCancellationTokenSource = new CancellationTokenSource();

        [SerializeField]
        private JumpDescentParameters _jumpDescentParameters;

        [SerializeField]
        private bool _descentUntilCancelled = false;

        public event EventHandler MotionStarted;
        public event EventHandler MotionEnded;

        [field: SerializeField]
        public UnityEvent JumpDescentStarted { get; private set; }

        [field: SerializeField]
        public UnityEvent JumpDescentEnded { get; private set; }

        private bool _inMotion;
        public bool InMotion
        {
            get => _inMotion;
            set
            {
                bool wasInMotion = _inMotion;
                _inMotion = value;

                if (!wasInMotion && _inMotion)
                    JumpDescentStarted?.Invoke();
                else if (wasInMotion && !_inMotion)
                    JumpDescentEnded?.Invoke();
            }
        }

        private void Awake()
        {
            _jumpDescentTask = Task.FromResult(true);

            _accelerationMovementPerformer = GetComponentInChildren<IMovementPerformer<Acceleration>>();

            JumpDescentStarted.AddListener(() => MotionStarted?.Invoke(this, EventArgs.Empty));
            JumpDescentEnded.AddListener(() => MotionEnded?.Invoke(this, EventArgs.Empty));
        }

        private void OnDestroy()
        {
            JumpDescentStarted.RemoveAllListeners();
            JumpDescentEnded.RemoveAllListeners();
        }

        public Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input)
        {
            return TryStartJumpDescentTask(readOnlyRigidbody, rigidbody, input);
        }

        private Task<bool> TryStartJumpDescentTask(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input) =>
            InMotion
            ? _jumpDescentTask
            : (_jumpDescentTask = JumpDescentAsync(readOnlyRigidbody, rigidbody, input, _jumpDescentCancellationTokenSource.Token));

        public void Cancel() => TryCancel();
        public void Precancel() => _jumpDescentCancellationTokenSource.Cancel();
        public bool TryCancel()
        {
            if (InMotion)
            {
                _jumpDescentCancellationTokenSource.Cancel();
                _jumpDescentCancellationTokenSource.Dispose();
                _jumpDescentCancellationTokenSource = new CancellationTokenSource();
                return true;
            }

            return false;
        }

        private async Task<bool> JumpDescentAsync(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input, CancellationToken cancellationToken)
        {
            InMotion = true;
            Acceleration gravity = input * _jumpDescentParameters.DescentGravityMagnitude - readOnlyRigidbody.GravityScale * Physics.gravity;
            bool cancelled = false;
            for (float time = 0.0f; (_descentUntilCancelled || time < _jumpDescentParameters.DescentTime) && !cancelled; time += Time.fixedDeltaTime)
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
