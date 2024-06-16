using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuctionChurch.UtilComponents.Animation
{
    public class AnimationParameterEventHandler : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetBoolTrue(string parameterName)
        {
            _animator.SetBool(parameterName, true);
        }

        public void SetBoolFalse(string parameterName)
        {
            _animator.SetBool(parameterName, false);
        }

        public void ToggleBool(string parameterName)
        {
            int hash = Animator.StringToHash(parameterName);
            bool current = _animator.GetBool(hash);

            _animator.SetBool(hash, !current);
        }

        public void SetTrigger(string parameterName)
        {
            _animator.SetTrigger(parameterName);
        }
    }
}