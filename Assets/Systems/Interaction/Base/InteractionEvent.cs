using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction
{
    public class InteractionEvent : Interactable
    {
        [SerializeField] private UnityEvent _unityEvent;

        public override void Interact()
        {
            _unityEvent.Invoke();
        }
    }
}