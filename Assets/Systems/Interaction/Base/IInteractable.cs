using System;
using UnityEngine;

namespace AuctionChurch.Interaction
{
    // generic visitor known by the visitables
    public interface IInteractor
    {
        public void Interact<T>(T interactable) where T : IInteractable;
    }

    // specific visitor that actually implements the functionality
    public interface IInteractor<T> where T :IInteractable
    {
        public IInteractor GeneralInteractor { get; }

        public void Interact(T interactable);
    }

    // visitable that implements functionality for when it is accepted
    public interface IInteractable
    {
        public void Accept(IInteractor interactor);
    }

    public abstract class Interactor : MonoBehaviour
    {
        public Action<IInteractable> OnInteract;

        // concrete wrapper for a specific visitor
        private class InteractorWrapper : IInteractor
        {
            private readonly object _specificInteractor;

            public InteractorWrapper(object specificInteractor)
            {
                _specificInteractor = specificInteractor;
            }

            public void Interact<T>(T interactable) where T : IInteractable
            {
                IInteractor<T> i = _specificInteractor as IInteractor<T>;

                i?.Interact(interactable);
            }
        }

        public IInteractor GeneralInteractor { get; private set; }

        protected virtual void Awake()
        {
            GeneralInteractor = new InteractorWrapper(this);
        }

        protected void Interact(IInteractable interactable)
        {
            OnInteract?.Invoke(interactable);
            interactable.Accept(GeneralInteractor);
        }
    }
}