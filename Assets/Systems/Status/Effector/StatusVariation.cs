using UnityEngine;
using UnityEngine.Events;

namespace StatusSystem.Effector
{
    [CreateAssetMenu(menuName = "Status System/Effector/Status Variation")]
    public class StatusVariation : ScriptableObject
    {
        [SerializeField]
        private float _defaultVariation = 0.0f;
        public float Variation { get; set; }

        [field: SerializeField]
        public UnityEvent<float> VariationApplied { get; private set; }

        private void OnEnable()
        {
            VariationApplied ??= new UnityEvent<float>();
            Variation = _defaultVariation;
        }

        public float Vary(ClampedStatus status)
        {
            status.Value += Variation;
            VariationApplied.Invoke(status.Value);
            return status.Value;
        }

        public float Vary(Status status)
        {
            status.Value += Variation;
            VariationApplied.Invoke(status.Value);
            return status.Value;
        }
    }
}