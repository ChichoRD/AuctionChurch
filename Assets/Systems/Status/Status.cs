using System;
using UnityEngine;
using UnityEngine.Events;

namespace StatusSystem
{
    [Serializable]
    internal readonly struct Status<T>
    {
        public readonly T value;

        public Status(T value, Action<T> valueSet) : this()
        {
            this.value = value;
            valueSet.Invoke(value);
        }
    }

    [CreateAssetMenu(fileName = "New Status", menuName = "Status System/Status")]
    public class Status : ScriptableObject
    {
        private Status<float> _status;
        public float Value
        {
            get => _status.value;
            set => _status = new Status<float>(value, ValueSet.Invoke);
        }

        [field: SerializeField]
        public UnityEvent<float> ValueSet { get; private set; }

        [SerializeField]
        private float _defaultValue;

        private void OnEnable()
        {
            ValueSet ??= new UnityEvent<float>();
            Value = _defaultValue;
        }
    }
}