using AuctionChurch.UtilComponents.Physics;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectHolder : MonoBehaviour
    {
        [SerializeField] private InputActionReference _dropInput;

        [Header("References")]
        [SerializeField] private Transform _objectParent;
        [SerializeField] private ObjectPlacer _objectPlacer;
        private Transform _previousParent;

        [Header("Layers")]
        [SerializeField] private LayerMask _heldLayerMask;
        [SerializeField] private LayerSwitcher _layerSwitcher;

        public Action OnHold;
        public Action OnRelease;
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

            RecordObject();
            UpdateObject();

            OnHold?.Invoke();
        }

        private void Release(InputAction.CallbackContext ctx) => Release();
        public void Release()
        {
            if (HeldObject == null)
                return;

            _layerSwitcher.RestoreObjectLayers();
            _layerSwitcher.ClearObject();

            ResolveReleasePosition();
            HeldObject.Release();
            HeldObject = null;

            OnRelease?.Invoke();
        }

        private void UpdateObject()
        {
            _layerSwitcher.SwitchLayer(_heldLayerMask);
            HeldObject.transform.parent = _objectParent;
            HeldObject.Hold();
        }

        private void RecordObject()
        {
            _previousParent = HeldObject.transform.parent;
            _layerSwitcher.RecordObject(HeldObject.gameObject);
        }

        private void ResolveReleasePosition()
        {
            HeldObject.transform.parent = _previousParent;
            HeldObject.transform.rotation = Quaternion.identity;
            _objectPlacer.PlaceObject(HeldObject.transform);
        }
    }
}