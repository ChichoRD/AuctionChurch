using AuctionChurch.Interaction.Detection;
using AuctionChurch.Interaction.Doors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction.Interactors
{
    public class DoorInteractor : Interactor, IInteractor<Door>
    {
        [SerializeField] private InteractionDetector _detector;

        public void TryInteract()
        {
            IInteractable[] interactables = _detector.Detect();

            if (interactables == null)
                return;

            for (int i = 0; i < interactables.Length; i++)
                Interact(interactables[i]);
        }

        public void Interact(Door door)
        {
            door.ToggleOpen();
        }
    }
}