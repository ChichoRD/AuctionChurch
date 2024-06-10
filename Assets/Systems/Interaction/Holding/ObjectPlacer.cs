using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private bool _drawGizmos = true;
        [SerializeField] private float _placementRange;

        public void PlaceObject(GameObject obj)
        {
            obj.transform.position = GetPlacementPosition(obj.transform);
            obj.transform.rotation = Quaternion.identity;
        }

        private Vector3 GetPlacementPosition(Transform obj)
        {
            Transform cam = Camera.main.transform;
            Ray ray = new(cam.position, cam.forward);

            Physics.Raycast(ray, out RaycastHit hit, _placementRange);

            if (hit.transform == null)
                return cam.transform.position + ray.direction * _placementRange;

            return GetPositionOutsideColliders(obj, hit);
        }

        private Vector3 GetPositionOutsideColliders(Transform obj, RaycastHit hit)
        {
            Collider objCollider = obj.GetComponent<Collider>();
            Collider hitCollider = hit.collider;
            Transform hitTransform = hit.transform;

            bool validComputation = Physics.ComputePenetration(objCollider, hit.point, obj.rotation,
                                            hitCollider, hitTransform.position, hitTransform.rotation,
                                            out Vector3 direction, out float distance);

            if (validComputation)
                return hit.point + (direction * distance);
            else
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