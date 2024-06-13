using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectPlacer : MonoBehaviour
    {
        [Header("Placing")]
        [SerializeField] private bool _drawGizmos = true;
        [SerializeField] private float _placementRange;

        public Action OnPlace { get; set; }
        public Action OnDrop { get; set; }

        public void PlaceObject(GameObject obj)
        {
            obj.transform.position = GetPlacementPosition(obj.transform);
        }

        private Vector3 GetPlacementPosition(Transform obj)
        {
            Transform cam = Camera.main.transform;
            Ray ray = new(cam.position, cam.forward);

            Physics.Raycast(ray, out RaycastHit hit, _placementRange);

            if (hit.transform == null)
            {
                OnDrop?.Invoke();
                return obj.position;
            }

            OnPlace?.Invoke();
            obj.transform.rotation = Quaternion.identity;
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