using AuctionChurch.Interaction.Detection;
using AuctionChurch.Interaction.Holding;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction
{
    public class PlayerInteractor : Interactor, IInteractor<HoldableObject>
    {
        [Header("References")]
        [SerializeField] private InputActionReference _interactionInput;
        [SerializeField] private InteractionDetector _detector;
        [SerializeField] private ObjectHolder _objectHolder;

        private void OnEnable()
        {
            if (!_interactionInput.action.enabled)
                _interactionInput.action.Enable();

            _interactionInput.action.performed += TryInteract;
        }

        private void OnDisable()
        {
            if (_interactionInput.action.enabled)
                _interactionInput.action.Disable();

            _interactionInput.action.performed -= TryInteract;
        }

        public void TryInteract(InputAction.CallbackContext context)
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