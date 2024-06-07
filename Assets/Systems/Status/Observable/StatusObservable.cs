using UnityEngine;
using UnityEngine.Events;

namespace StatusSystem.Observable
{
    public class StatusObservable : MonoBehaviour
    {
        [SerializeField]
        private Status _status;

        [field: SerializeField]
        public UnityEvent<float> ValueSet { get; private set; }

        private void Awake()
        {
            ValueSet ??= new UnityEvent<float>();

            _status.ValueSet.AddListener(ValueSet.Invoke);
        }

        private void OnDestroy()
        {
            _status.ValueSet.RemoveListener(ValueSet.Invoke);
        }
    }
}