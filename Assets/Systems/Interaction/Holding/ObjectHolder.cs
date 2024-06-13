using System;
using UnityEngine;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectHolder : MonoBehaviour
    {
        [SerializeField] private Transform _holdingParent;
        private Transform _previousParent;

        public Action<HoldableObject> OnHold { get; set; }
        public Action<HoldableObject> OnRelease { get; set; }
        public HoldableObject HeldObject { get; private set; }

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

        public void Release()
        {
            if (HeldObject == null)
                return;

            RevertObject();

            OnRelease?.Invoke(HeldObject);
            HeldObject = null;
        }

        private void RevertObject()
        {
            HeldObject.transform.parent = _previousParent;
            HeldObject.Release();
        }
    }
}