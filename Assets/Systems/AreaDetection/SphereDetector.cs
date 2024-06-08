using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.AreaDetection
{
    public class SphereDetector : AreaDetector
    {
        [SerializeField] private float radius = 0.5f;

        private int hits;
        private const int MAX_COLLISIONS = 10;
        private readonly Collider[] collisions = new Collider[MAX_COLLISIONS];

        public float Radius => radius;

        public override int Detect()
        {
            QueryTriggerInteraction query = detectTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

            hits = Physics.OverlapSphereNonAlloc(Offset, radius, collisions, layerMask, query);

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
            Gizmos.DrawWireSphere(Offset, radius);
        }
    }
}