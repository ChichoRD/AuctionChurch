using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction.Holding
{
    public class HoldableObject : MonoBehaviour, IInteractable
    {
        [Header("Held Offsets")]
        [SerializeField] private Vector3 _heldPositionOffset;
        [SerializeField] private Vector3 _heldRotationOffset;

        public Vector3 HeldPositionOffset => _heldPositionOffset;
        public Quaternion HeldRotationOffset => Quaternion.Euler(_heldRotationOffset);

        [Header("Events")]
        [SerializeField] private UnityEvent _onHold;
        [SerializeField] private UnityEvent _onRelease;

        public void Accept(IInteractor interactor) => interactor.Interact(this);

        public void Hold()
        {
            _onHold.Invoke();
        }

        public void Release()
        {
            _onRelease.Invoke(); 
        }
    }
}