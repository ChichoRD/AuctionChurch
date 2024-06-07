using UnityEngine;

namespace StatusSystem.Effector
{
    [CreateAssetMenu(menuName = "Status System/Effector/Status Variation")]
    public class StatusVariation : ScriptableObject
    {
        [SerializeField]
        private float _defaultVariation = 0.0f;
        public float Variation { get; set; }

        private void OnEnable()
        {
            Variation = _defaultVariation;
        }

        public float Vary(ClampedStatus status)
        {
            status.Value += Variation;
            return status.Value;
        }

        public float Vary(Status status)
        {
            status.Value += Variation;
            return status.Value;
        }
    }
}