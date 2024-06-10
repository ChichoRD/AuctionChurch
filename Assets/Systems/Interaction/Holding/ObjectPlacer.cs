using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private bool _drawGizmos = true;
        [SerializeField] private float _placementRange;

        public void PlaceObject(Transform obj)
        {
            obj.position = GetPlacementPosition();
        }

        private Vector3 GetPlacementPosition()
        {
            Transform cam = Camera.main.transform;
            Ray ray = new(cam.position, cam.forward);

            Physics.Raycast(ray, out RaycastHit hit, _placementRange);

            if (hit.transform == null)
                return cam.transform.position + ray.direction * _placementRange;

            return hit.point;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_drawGizmos)
                return;

            Transform cam = Camera.main.transform;

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(cam.position, cam.forward * _placementRange);
        }
    }
}