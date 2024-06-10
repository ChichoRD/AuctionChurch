using AuctionChurch.Interaction;
using AuctionChurch.Interaction.Detection;
using StatusSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StatusInteractionSystem.Variation
{
    internal class StatusVariationInteractor : Interactor, IInteractor<StatusVariationInteractable>
    {
        [SerializeField]
        private ClampedStatus _status;

        [SerializeField]
        private InteractionDetector _detector;

        [SerializeField]
        private InputActionReference _variationInputActionReference;

        private void Awake()
        {
            _variationInputActionReference.action.performed += OnVariationInputPerformed;
        }

        private void OnEnable()
        {
            if (!_variationInputActionReference.action.enabled)
                _variationInputActionReference.action.Enable();
        }

        private void OnDisable()
        {
            if (_variationInputActionReference.action.enabled)
                _variationInputActionReference.action.Disable();
        }

        private void OnDestroy()
        {
            _variationInputActionReference.action.performed -= OnVariationInputPerformed;
        }

        private void OnVariationInputPerformed(InputAction.CallbackContext context)
        {
            foreach (IInteractable interactable in _detector.Detect())
                Interact(interactable);
        }

        public void Interact(StatusVariationInteractable interactable) =>
            interactable.VaryStatus(_status);
    }
}
