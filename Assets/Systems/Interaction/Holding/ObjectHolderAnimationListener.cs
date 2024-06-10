using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectHolderAnimationListener : MonoBehaviour
    {
        [Header("Holder")]
        [SerializeField] private ObjectHolder _holder;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animationParameter;
        private int _animationParameterHash;

        private void OnEnable()
        {
            _animationParameterHash = Animator.StringToHash(_animationParameter);

            _holder.OnHold += OnHold;
            _holder.OnRelease += OnRelease;
        }

        private void OnDisable()
        {
            _holder.OnHold -= OnHold;
            _holder.OnRelease -= OnRelease;
        }

        private void OnHold()
        {
            _animator.SetBool(_animationParameterHash, true);
        }

        private void OnRelease() 
        {
            _animator.SetBool(_animationParameterHash, false);
        }
    }
}