using SH.AreaDetection;
using System;
using UnityEngine;

namespace AuctionChurch.Interaction
{
    [RequireComponent(typeof(RayDetector))]
    public class InteractionDetector : MonoBehaviour
    {
        private RayDetector _detector;

        private void Awake()
        {
            _detector = GetComponent<RayDetector>();
        }

        public IInteractable[] Detect()
        {
            if (_detector.Detect() == 0)
                return Array.Empty<IInteractable>();

            Collider hit = _detector.GetHit();

            if (hit == null)
                return Array.Empty<IInteractable>();

            return hit.GetComponents<IInteractable>();
        }
    }
}