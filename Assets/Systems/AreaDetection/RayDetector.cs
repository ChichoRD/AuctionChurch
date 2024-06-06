using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.AreaDetection
{
    public class RayDetector : AreaDetector
    {
        [SerializeField] private float range = 1;
        [SerializeField] private Vector3 direction = Vector3.forward;

        private const int MAX_COLLISIONS = 1;
        private readonly RaycastHit[] collisions = new RaycastHit[MAX_COLLISIONS];

        public Vector3 Direction => transform.TransformDirection(direction).normalized;

        public override int Detect()
        {
            QueryTriggerInteraction query = detectTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

            return Physics.RaycastNonAlloc(Offset, Direction, collisions, range, layerMask, query);
        }

        public override Collider GetHit()
        {
            return collisions[0].collider;
        }

        public override Collider[] GetHits()
        {
            Collider[] colliders = new Collider[collisions.Length];
            
            for (int i = 0; i < collisions.Length; i++)
            {
                colliders[i] = collisions[i].collider;
            }

            return colliders;
        }

        public RaycastHit GetRaycastHit()
        {
            return collisions[0];
        }

        protected override void DrawGizmos()
        {
            Gizmos.DrawRay(Offset, Direction * range);
        }
    }
}