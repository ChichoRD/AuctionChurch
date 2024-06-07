using UnityEngine;
using UnityEngine.Events;

namespace StatusSystem.Observable
{
    public class ClampedStatusObservable : MonoBehaviour
    {
        [SerializeField]
        private ClampedStatus _clampedStatus;

        [field: SerializeField]
        public UnityEvent<float> ValueReachedMinThreshold { get; private set; }

        [field: SerializeField]
        public UnityEvent<float> ValueReachedMaxThreshold { get; private set; }

        [field: SerializeField]
        public UnityEvent<float> ValueSet { get; private set; }

        private void Awake()
        {
            ValueReachedMinThreshold ??= new UnityEvent<float>();
            ValueReachedMaxThreshold ??= new UnityEvent<float>();
            ValueSet ??= new UnityEvent<float>();

            _clampedStatus.ValueReachedMinThreshold.AddListener(ValueReachedMinThreshold.Invoke);
            _clampedStatus.ValueReachedMaxThreshold.AddListener(ValueReachedMaxThreshold.Invoke);
            _clampedStatus.ValueSet.AddListener(ValueSet.Invoke);
        }

        private void OnDestroy()
        {
            _clampedStatus.ValueReachedMinThreshold.RemoveListener(ValueReachedMinThreshold.Invoke);
            _clampedStatus.ValueReachedMaxThreshold.RemoveListener(ValueReachedMaxThreshold.Invoke);
            _clampedStatus.ValueSet.RemoveListener(ValueSet.Invoke);
        }
    }
}