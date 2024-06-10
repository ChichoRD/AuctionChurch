using Helpers.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AuctionChurch.UtilComponents.Physics
{
    // A simple component for recording an object's original layers and switching an object and all of its children's layers.
    // To use:
    // 1. Set LayerMask to the desired layer
    // 2. Record an object's original layers with RecordObject
    // 3. Set a new layer for the object and its children with SwitchLayer
    // 4. Restore the object's layers with RestoreObjectLayers
    public class LayerSwitcher : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        private Transform _currentObject;
        private readonly Dictionary<Transform, int> _originalLayers = new();

        public LayerMask LayerMask => _layerMask;

        public void RecordObject(GameObject obj)
        {
            if (obj == null)
                return;

            if (_currentObject != null)
                ClearObject();

            _currentObject = obj.transform;
            RecordObjectAndChildren(_currentObject);
        }

        private void RecordObjectAndChildren(Transform obj)
        {
            _originalLayers.Add(obj, obj.gameObject.layer);

            int childCount = obj.childCount;

            for (int i = 0; i < childCount; i++)
                RecordObjectAndChildren(obj.GetChild(i));
        }

        public void SwitchLayer() => SwitchLayersWithChildren(_currentObject, LayerMask.ToLayer());

        private void SwitchLayersWithChildren(Transform obj, int layer)
        {
            obj.gameObject.layer = layer;

            int childCount = obj.childCount;

            for (int i = 0; i < childCount; i++)
                SwitchLayersWithChildren(obj.GetChild(i), layer);
        }

        public void RestoreObjectLayers()
        {
            Transform[] objects = _originalLayers.Keys.ToArray();
            int[] layers = _originalLayers.Values.ToArray();

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].gameObject.layer = layers[i];
            }
        }

        public void ClearObject()
        {
            _currentObject = null;
            _originalLayers.Clear();
        }
    }
}