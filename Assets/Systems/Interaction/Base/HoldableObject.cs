using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuctionChurch.Interaction.Holding
{
    public class HoldableObject : MonoBehaviour, IInteractable
    {
        private Rigidbody _rb;
        private Collider[] _colliders;
        private bool _isHeld;

        public bool IsHeld 
        { 
            get => _isHeld; 
            set
            {
                _isHeld = value;
                UpdatePhysicsComponents();
            }
        }

        private void Awake()
        {
            _rb = GetComponentInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
        }

        public void Accept(IInteractor interactor)
        {
            interactor.Interact(this);
        }

        private void UpdatePhysicsComponents()
        {
            if (_rb != null)
                _rb.isKinematic = _isHeld;

            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = !_isHeld;
            }
        }
    }
}