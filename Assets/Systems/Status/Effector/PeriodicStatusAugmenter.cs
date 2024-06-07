using System.Collections;
using UnityEngine;

namespace StatusSystem.Effector
{
    public class PeriodicStatusAugmenter : MonoBehaviour
    {
        [SerializeField]
        [Min(0.0f)]
        private float _updateIntervalSeconds = 1.0f;
        private WaitForSeconds _updateIntervalWait;

        [SerializeField]
        private float _defaultVariation = 0.0f;
        public float Variation { get; set; }

        [field: SerializeField]
        public ClampedStatus Status { get; private set; }

        [SerializeField]
        private bool _beginOnStart = true;
        private Coroutine _augmentStatusCoroutine;

        private void Awake()
        {
            Variation = _defaultVariation;
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
                Status.Value += Variation;
                yield return _updateIntervalWait;
            }
        }
    }
}