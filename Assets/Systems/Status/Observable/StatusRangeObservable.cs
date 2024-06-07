using UnityEngine;
using UnityEngine.Events;

namespace StatusSystem.Observable
{
    public class StatusRangeObservable : MonoBehaviour
    {
        [SerializeField]
        private Status _status;

        [SerializeField]
        private float _minValueThreshold;
        [SerializeField]
        private float _maxValueThreshold;

        [field: SerializeField]
        public UnityEvent<float> ValueReachedMinThreshold { get; private set; }

        [field: SerializeField]
        public UnityEvent<float> ValueReachedMaxThreshold { get; private set; }

        private void Awake()
        {
            ValueReachedMinThreshold ??= new UnityEvent<float>();
            ValueReachedMaxThreshold ??= new UnityEvent<float>();

            _status.ValueSet.AddListener(OnValueChanged);
        }

        private void OnDestroy()
        {
            _status.ValueSet.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            if (value <= _minValueThreshold)
            {
                ValueReachedMinThreshold.Invoke(value);
            }
            else if (value >= _maxValueThreshold)
            {
                ValueReachedMaxThreshold.Invoke(value);
            }
        }
    }
}