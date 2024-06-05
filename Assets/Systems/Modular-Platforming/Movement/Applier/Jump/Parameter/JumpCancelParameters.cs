using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Jump.Parameter
{
    [CreateAssetMenu(fileName = "JumpCancelParameters", menuName = "Modular Platforming/Movement/Applier/Jump/Parameter/Jump Cancel Parameters")]
    internal class JumpCancelParameters : ScriptableObject
    {
        [SerializeField]
        [Min(0.02f)]
        private float _jumpCancelTime = 0.1f;

        public float JumpCancelTime => _jumpCancelTime;
    }
}