using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.AreaDetection
{
    public class BoxDetector : AreaDetector
    {
        [SerializeField] private Vector3 halfExtents = Vector3.one;
        [SerializeField] private Vector3 rotation;

        private int hits;
        private const int MAX_COLLISIONS = 10;
        private readonly Collider[] collisions = new Collider[MAX_COLLISIONS];

        public override int Detect()
        {
            QueryTriggerInteraction query = detectTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

            hits = Physics.OverlapBoxNonAlloc(Offset, halfExtents, collisions, Quaternion.Euler(rotation), layerMask, query);

            return hits;
        }

        public override Collider GetHit()
        {
            return collisions[0];
        }

        public override Collider[] GetHits()
        {
            if (hits == 0)
                return null;

            Collider[] subArr = new Collider[hits];

            for (int i = 0; i < hits; i++)
                subArr[i] = collisions[i];

            return subArr;
        }

        protected override void DrawGizmos()
        {
            Gizmos.matrix *= Matrix4x4.Rotate(Quaternion.Euler(rotation));

            Gizmos.DrawWireCube(Offset, halfExtents * 2);
        }
    }
}