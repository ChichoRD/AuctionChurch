using AuctionChurch.Interaction.Holding;
using SH.AreaDetection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction
{
    public class PlayerInteractor : Interactor, IInteractor<HoldableObject>
    {
        [SerializeField] private InputActionReference _interactionInput;
        private InteractionDetector _detector;
        private ItemHolder _itemHolder;

        protected override void Awake()
        {
            base.Awake();
            _detector = GetComponentInChildren<InteractionDetector>();
            _itemHolder = GetComponentInChildren<ItemHolder>();
        }

        private void OnEnable()
        {
            _interactionInput.action.Enable();
            _interactionInput.action.performed += TryInteract;
        }

        private void OnDisable()
        {
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

        void IInteractor<HoldableObject>.Interact(HoldableObject interactable)
        {
            if (_itemHolder.IsHolding)
                return;

            _itemHolder.Hold(interactable);
        }
    }
}