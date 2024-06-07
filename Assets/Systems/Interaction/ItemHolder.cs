using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction.Holding
{
    public class ItemHolder : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference _dropInput;

        [Header("Positioning")]
        [SerializeField] private bool _drawGizmos = true;
        [SerializeField] private Vector3 _holdOffset;
        private HoldableObject _heldItem;

        public bool IsHolding => _heldItem != null;

        private void OnEnable()
        {
            _dropInput.action.Enable();
            _dropInput.action.performed += Drop;
        }

        private void OnDisable()
        {
            _dropInput.action.Disable();
            _dropInput.action.performed -= Drop;
        }

        public void Hold(HoldableObject item)
        {
            _heldItem = item;

            item.IsHeld = true;
            item.transform.parent = transform;
            item.transform.localPosition = _holdOffset;
        }

        public void Drop(InputAction.CallbackContext ctx)
        {
            if (_heldItem == null)
                return;

            _heldItem.IsHeld = false;
            _heldItem.transform.parent = null;

            _heldItem = null;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_drawGizmos)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.TransformPoint(_holdOffset), 0.35f);
        }
    }
}