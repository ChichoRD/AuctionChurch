using SH.AreaDetection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction
{
    public class PlayerInteractor : Interactor
    {
        [SerializeField] private InputActionReference _interactionInput;
        private InteractionDetector _detector;

        protected override void Awake()
        {
            base.Awake();
            _detector = GetComponentInChildren<InteractionDetector>();
        }

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
    }
}