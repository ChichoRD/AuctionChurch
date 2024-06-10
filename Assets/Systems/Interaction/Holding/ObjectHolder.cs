using AuctionChurch.UtilComponents.Physics;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectHolder : MonoBehaviour
    {
        [SerializeField] private InputActionReference _dropInput;
        [SerializeField] private Transform _holdingParent;
        private Transform _previousParent;

        public Action<HoldableObject> OnHold { get; set; }
        public Action<HoldableObject> OnRelease { get; set; }
        public HoldableObject HeldObject { get; private set; }

        private void OnEnable()
        {
            _dropInput.action.Enable();
            _dropInput.action.performed += Release;
        }

        private void OnDisable()
        {
            _dropInput.action.Disable();
            _dropInput.action.performed -= Release;
        }

        public void Hold(HoldableObject obj)
        {
            if (HeldObject != null)
                return;

            HeldObject = obj;

            _previousParent = HeldObject.transform.parent;
            HeldObject.transform.parent = _holdingParent;
            HeldObject.Hold();

            OnHold?.Invoke(obj);
        }

        private void Release(InputAction.CallbackContext ctx) => Release();
        public void Release()
        {
            if (HeldObject == null)
                return;

            HeldObject.transform.parent = _previousParent;
            OnRelease?.Invoke(HeldObject);
            HeldObject.Release();
            HeldObject = null;
        }
    }
}