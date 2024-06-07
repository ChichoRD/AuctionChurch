using SH.AreaDetection;
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
                return null;

            Collider hit = _detector.GetHit();

            if (hit == null)
                return null;

            return hit.GetComponents<IInteractable>();
        }
    }
}