using SH.AreaDetection;
using System;
using UnityEngine;

namespace AuctionChurch.Interaction.Detection
{
    public abstract class InteractionDetector : MonoBehaviour
    {
        public abstract IInteractable[] Detect();
    }
}