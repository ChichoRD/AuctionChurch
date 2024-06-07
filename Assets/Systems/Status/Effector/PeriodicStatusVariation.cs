using System.Collections;
using UnityEngine;

namespace StatusSystem.Effector
{
    public class PeriodicStatusVariation : MonoBehaviour
    {
        [SerializeField]
        [Min(0.0f)]
        private float _updateIntervalSeconds = 1.0f;
        private WaitForSeconds _updateIntervalWait;
        private Coroutine _augmentStatusCoroutine;

        [field: SerializeField]
        public StatusVariation Variation { get; set; }

        [field: SerializeField]
        public ClampedStatus Status { get; set; }

        [SerializeField]
        private bool _beginOnStart = true;

        private void Awake()
        {
            _updateIntervalWait = new WaitForSeconds(_updateIntervalSeconds);
        }

        private void Start()
        {
            _ = _beginOnStart && TryStartAugmentingStatus();
        }

        public bool TryStopAugmentingStatus()
        {
            if (_augmentStatusCoroutine != null)
            {
                StopCoroutine(_augmentStatusCoroutine);
                _augmentStatusCoroutine = null;
                return true;
            }

            return false;
        }

        public bool TryStartAugmentingStatus()
        {
            if (_augmentStatusCoroutine == null)
            {
                _augmentStatusCoroutine = StartCoroutine(AugmentStatusCoroutine());
                return true;
            }

            return false;
        }

        private IEnumerator AugmentStatusCoroutine()
        {
            while (enabled)
            {
                Variation.Vary(Status);
                yield return _updateIntervalWait;
            }
        }
    }
}