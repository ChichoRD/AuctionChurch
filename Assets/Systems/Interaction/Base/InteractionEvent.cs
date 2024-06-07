using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction
{
    public class InteractionEvent : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent _unityEvent;

        public void Accept(IInteractor interactor)
        {
            Debug.Log("event fired!");

            _unityEvent.Invoke();
            interactor.Interact(this);
        }
    }
}