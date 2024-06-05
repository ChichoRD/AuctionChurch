using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Jump.Parameter
{
    [CreateAssetMenu(fileName = "JumpDescentParameters", menuName = "Modular Platforming/Movement/Applier/Jump/Parameter/Jump Descent Parameters")]
    internal class JumpDescentParameters : ScriptableObject
    {
        [SerializeField]
        private float _descentHeight = 2.7f;

        [SerializeField]
        [Min(0.02f)]
        private float _descentTime = 0.36f;

        public float DescentHeight => _descentHeight;
        public float DescentTime => _descentTime;
        public float DescentGravityMagnitude => -2.0f * _descentHeight / (_descentTime * _descentTime);
    }
}