using AuctionChurch.Interaction.Detection;
using AuctionChurch.Interaction.Holding;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction.Interactors
{
    public class HoldableObjectInteractor : Interactor, IInteractor<HoldableObject>
    {
        [Header("References")]
        [SerializeField] private InteractionDetector _detector;
        [SerializeField] private ObjectHolder _objectHolder;

        public void TryInteract()
        {
            IInteractable[] interactables = _detector.Detect();

            if (interactables == null)
                return;

            for (int i = 0; i < interactables.Length; i++)
                Interact(interactables[i]);
        }

        public void Interact(HoldableObject interactable)
        {
            _objectHolder.Hold(interactable);
        }
    }
}