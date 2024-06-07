using UnityEngine;
using UnityEngine.Events;

namespace StatusSystem
{
    [CreateAssetMenu(fileName = "New Clamped Status", menuName = "Status System/Clamped Status")]
    public class ClampedStatus : ScriptableObject
    {
        [SerializeField]
        private Status _status;
        public float Value
        {
            get => _status.Value;
            set
            {
                if (value < _minValue)
                {
                    _status.Value = _minValue;
                    ValueReachedMinThreshold?.Invoke(_minValue);
                }
                else if (value > _maxValue)
                {
                    _status.Value = _maxValue;
                    ValueReachedMaxThreshold?.Invoke(_maxValue);
                }
                else
                {
                    _status.Value = value;
                }
            }
        }

        [SerializeField]
        private float _minValue;

        [SerializeField]
        private float _maxValue;

        [field: SerializeField]
        public UnityEvent<float> ValueReachedMinThreshold { get; private set; }

        [field: SerializeField]
        public UnityEvent<float> ValueReachedMaxThreshold { get; private set; }

        private void OnEnable()
        {
            ValueReachedMinThreshold ??= new UnityEvent<float>();
            ValueReachedMaxThreshold ??= new UnityEvent<float>();
        }
    }
}