using UnityEngine;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectHolderAnimationListener : MonoBehaviour
    {
        [Header("Holder")]
        [SerializeField] private ObjectHolder _holder;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _isHoldingAnimationParameter;
        [SerializeField] private string _grabAnimationParameter;
        private int _isHoldingHash;
        private int _grabHash;

        private void OnEnable()
        {
            _isHoldingHash = Animator.StringToHash(_isHoldingAnimationParameter);
            _grabHash = Animator.StringToHash(_grabAnimationParameter);

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
            _animator.SetTrigger(_grabHash);
            _animator.SetBool(_isHoldingHash, true);
        }

        private void OnRelease() 
        {
            _animator.SetTrigger(_grabHash);
            _animator.SetBool(_isHoldingHash, false);
        }
    }
}