using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction
{
    public class InteractorListener : MonoBehaviour
    {
        [Header("Interactor")]
        [SerializeField] private Interactor _interactor;

        [Header("Events")]
        [SerializeField] private UnityEvent _event;

        private void OnEnable()
        {
            _interactor.OnInteract += (IInteractable interactable) => RaiseEvents();
        }

        private void OnDisable()
        {
            _interactor.OnInteract -= (IInteractable interactable) => RaiseEvents();
        }

        private void RaiseEvents() => _event.Invoke();
    }
}