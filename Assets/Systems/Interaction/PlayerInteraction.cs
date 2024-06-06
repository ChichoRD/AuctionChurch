using SH.AreaDetection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionReference _interactionActionReference;
        private InteractionDetector _detector;

        private void Awake()
        {
            _detector = GetComponentInChildren<InteractionDetector>();
        }

        private void OnEnable()
        {
            _interactionActionReference.action.Enable();

            _interactionActionReference.action.performed += Interact;
        }

        private void OnDisable()
        {
            _interactionActionReference.action.Disable();

            _interactionActionReference.action.performed -= Interact;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            IInteractable interactable = _detector.Detect();

            interactable?.Interact();
        }
    }
}