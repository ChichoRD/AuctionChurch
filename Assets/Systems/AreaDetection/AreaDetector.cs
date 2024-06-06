using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.AreaDetection
{
    public abstract class AreaDetector : MonoBehaviour
    {
        [Header("Gizmos")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Color gizmosColor = Color.red;

        [Header("Detection")]
        [SerializeField] protected LayerMask layerMask = 1;
        [SerializeField] protected bool detectTriggers;
        [SerializeField] private bool localOffset = true;
        [SerializeField] private Vector3 offset = Vector3.zero;

        public LayerMask LayerMask { get => layerMask; set => layerMask = value; }
        public bool DetectTriggers { get => detectTriggers; set => detectTriggers = value; }
        public Vector3 Offset 
        { 
            get
            {
                if (localOffset) return transform.TransformPoint(offset);
                else return offset;
            }
            set => offset = value;
        }
        public bool LocalOffset { get => localOffset; set => localOffset = value; }

        public abstract int Detect();
        public abstract Collider GetHit();
        public abstract Collider[] GetHits();

        protected abstract void DrawGizmos();

        public void OnDrawGizmosSelected()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = gizmosColor;
            DrawGizmos();
        }
    }
}