using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction.Holding
{
    public class HoldableObject : MonoBehaviour, IInteractable
    {
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