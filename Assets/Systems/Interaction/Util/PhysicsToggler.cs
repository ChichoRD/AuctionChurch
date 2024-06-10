using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AuctionChurch.UtilComponents.Physics
{
    public class PhysicsToggler : MonoBehaviour
    {
        private Rigidbody[] _rigidBodies;
        private Collider[] _colliders;

        private void Awake()
        {
            Transform highestParent = transform;

            while (transform.parent != null)
                highestParent = transform.parent;

            _rigidBodies = highestParent.GetComponentsInChildren<Rigidbody>();
            _colliders = highestParent.GetComponentsInChildren<Collider>();
        }

        public void EnablePhysics() => TogglePhysics(true);

        public void DisablePhysics() => TogglePhysics(false);

        private void TogglePhysics(bool enabled)
        {
            for (int i = 0; i < _rigidBodies.Length; i++)
                _rigidBodies[i].isKinematic = !enabled;

            for (int i = 0; i < _colliders.Length; i++)
                _colliders[i].enabled = enabled;
        }
    }
}