using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuctionChurch.UtilComponents.Transforms
{
    // A functional component used to offset the local transform and revert it back to a previous state
    public class LocalOffsetter : MonoBehaviour
    {
        [SerializeField] private Vector3 _positionOffset;
        [SerializeField] private Vector3 _rotationOffset;
        [SerializeField] private Vector3 _scaleOffset = Vector3.one;
        private Vector3 _previousPosition;
        private Vector3 _previousRotation;
        private Vector3 _previousScale;

        public void ApplyPosition() => SetPosition(_positionOffset);
        public void ApplyRotation() => SetRotation(_rotationOffset);
        public void ApplyScale() => SetScale(_scaleOffset);

        public void ApplyOffsets()
        {
            ApplyPosition();
            ApplyRotation();
            ApplyScale();
        }

        public void RevertPosition() => SetPosition(_previousPosition);
        public void RevertRotation() => SetRotation(_previousRotation);
        public void RevertScale() => SetScale(_previousScale);

        public void RevertOffsets()
        {
            RevertPosition();
            RevertRotation();
            RevertScale();
        }

        private void SetPosition(Vector3 pos)
        {
            _previousPosition = transform.localPosition;
            transform.localPosition = pos;
        }

        private void SetRotation(Vector3 rot)
        {
            _previousRotation = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(rot);
        }

        private void SetScale(Vector3 scale)
        {
            _previousScale = transform.localScale;
            transform.localScale = scale;
        }
    }
}