using SH.AreaDetection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace AuctionChurch.Interaction
{
    [RequireComponent(typeof(SphereDetector), typeof(ParentConstraint))]
    public class InteractionDetector : MonoBehaviour
    {
        private SphereDetector _detector;
        private ParentConstraint _parentConstraint;

        private void Awake()
        {
            _detector = GetComponent<SphereDetector>();
            _parentConstraint = GetComponent<ParentConstraint>();
            ConstraintSource source = new()
            {
                sourceTransform = Camera.main.transform,
                weight = 1
            };
            _parentConstraint.SetSource(0, source);
        }

        public IInteractable Detect()
        {
            if (_detector.Detect() == 0)
                return null;

            Collider[] hits = _detector.GetHits();
            Interactable[] interactables = GetInteractables(hits);

            if (interactables.Length == 0)
                return null;

            return GetClosestInteractable(interactables);
        }

        private Interactable[] GetInteractables(Collider[] colliders)
        {
            List<Interactable> interactables = new(colliders.Length);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Interactable interactable))
                    interactables.Add(interactable);
            }

            return interactables.ToArray();
        }

        private Interactable GetClosestInteractable(Interactable[] interactables)
        {
            Vector3 parentPos = GetParentPosition();

            interactables = interactables
                                .OrderBy(t => (t.transform.position - parentPos).sqrMagnitude)
                                .ToArray();

            return GetFirstInteractableInSight(interactables);
        }

        private Interactable GetFirstInteractableInSight(Interactable[] interactables)
        {
            for (int i = 0; i < interactables.Length; i++)
            {
                Interactable interactable = interactables[i];

                if (InLineOfSight(interactable.transform))
                    return interactable;
            }

            return null;
        }

        private bool InLineOfSight(Transform target)
        {
            Vector3 origin = GetParentPosition();
            Vector3 heading = target.position - origin;
            Vector3 direction = heading.normalized;
            float distance = heading.magnitude;

            Physics.Raycast(origin, direction, out RaycastHit hit, distance);
            Debug.DrawRay(origin, direction * distance, Color.red, 5f);

            return hit.transform == target;
        }

        private Vector3 GetParentPosition() => _parentConstraint.GetSource(0).sourceTransform.position;
    }
}