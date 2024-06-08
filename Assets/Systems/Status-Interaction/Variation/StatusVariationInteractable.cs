using AuctionChurch.Interaction;
using StatusSystem;
using StatusSystem.Effector;
using UnityEngine;
using UnityEngine.Events;

namespace StatusInteractionSystem.Variation
{
    internal class StatusVariationInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private StatusVariation _statusVariation;
        [SerializeField]
        private UnityEvent _variedStatus;

        private void Awake()
        {
            _variedStatus ??= new UnityEvent();
        }

        public void Accept(IInteractor interactor) => interactor.Interact(this);

        public float VaryStatus(ClampedStatus status)
        {
            _variedStatus?.Invoke();
            return _statusVariation.Vary(status);
        }

        public float VaryStatus(Status status)
        {
            _variedStatus?.Invoke();
            return _statusVariation.Vary(status);
        }
    }
}