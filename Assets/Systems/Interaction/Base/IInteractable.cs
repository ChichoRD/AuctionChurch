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
        // concrete wrapper for a specific visitor
        private class InteractorWrapper : IInteractor
        {
            public Action<IInteractable> OnInteract;
            private readonly object _specificInteractor;

            public InteractorWrapper(object specificInteractor)
            {
                _specificInteractor = specificInteractor;
            }

            public void Interact<T>(T interactable) where T : IInteractable
            {
                IInteractor<T> i = _specificInteractor as IInteractor<T>;

                OnInteract?.Invoke(interactable);
                i?.Interact(interactable);
            }
        }

        public Action<IInteractable> OnInteract
        {
            get
            {
                _generalInteractor ??= new InteractorWrapper(this);

                return _generalInteractor.OnInteract;
            }
            set => _generalInteractor.OnInteract = value;
        }

        public IInteractor GeneralInteractor
        {
            get
            {
                _generalInteractor ??= new InteractorWrapper(this);

                return _generalInteractor;
            }
        }
        private InteractorWrapper _generalInteractor;

        protected void Interact(IInteractable interactable)
        {
            interactable.Accept(GeneralInteractor);
        }
    }
}