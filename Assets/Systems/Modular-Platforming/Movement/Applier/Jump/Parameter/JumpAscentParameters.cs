using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Jump.Parameter
{
    [CreateAssetMenu(fileName = "JumpAscentParameters", menuName = "Modular Platforming/Movement/Applier/Jump/Parameter/Jump Ascent Parameters")]
    internal class JumpAscentParameters : ScriptableObject
    {
        [SerializeField]
        private float _jumpHeight = 2.7f;

        [SerializeField]
        private float _minJumpHeight = 1.35f;

        [SerializeField]
        [Min(0.02f)]
        private float _jumpPeakReachTime = 0.36f;

        public float JumpHeight => _jumpHeight;
        public float MinJumpHeight => _minJumpHeight;
        public float JumpPeakReachTime => _jumpPeakReachTime;

        /* Kinematics:
         * dh = v0 * dt + 0.5 * a * dt^2
         * dv = a * dt
         * 
         * dh = v0 * dt + 0.5 * dv * dt
         * dh = (v0 + 0.5 * dv) * dt
         * 
         * dt = dh / (v0 + 0.5 * dv)
         * v0 = 0
         * 
         * dt = dh / (0.5 * dv)
         * dt = 2.0 * dh / dv
         */

        public float MinJumpHeightReachTime => Mathf.Sqrt(-2.0f * _minJumpHeight / JumpGravityMagnitude);
        public float JumpSpeed => 2.0f * _jumpHeight / _jumpPeakReachTime;
        public float JumpGravityMagnitude => -2.0f * _jumpHeight / (_jumpPeakReachTime * _jumpPeakReachTime);
    }
}