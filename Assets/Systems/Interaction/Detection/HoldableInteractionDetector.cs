using AuctionChurch.Interaction.Holding;
using System;
using UnityEngine;

namespace AuctionChurch.Interaction.Detection
{
    public class HoldableInteractionDetector : InteractionDetector
    {
        [SerializeField] private ObjectHolder _objectHolder;

        public override IInteractable[] Detect()
        {
            HoldableObject heldObject = _objectHolder.HeldObject;

            if (heldObject == null)
                return Array.Empty<IInteractable>();

            return heldObject.GetComponentsInChildren<IInteractable>();
        }
    }
}