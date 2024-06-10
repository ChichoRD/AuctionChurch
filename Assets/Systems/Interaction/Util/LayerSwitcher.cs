using Helpers.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AuctionChurch.UtilComponents.Physics
{
    // A simple component for recording an object's original layers and switching an object and all of its children's layers.
    // To use:
    // 1. Record an object's original layers with RecordObject
    // 2. Set a new layer for the object and its children with SwitchLayer
    // 3. Restore the object's layers with RestoreObjectLayers
    public class LayerSwitcher : MonoBehaviour
    {
        private GameObject _currentObject;
        private readonly Dictionary<GameObject, int> _originalLayers = new();

        public void RecordObject(GameObject obj)
        {
            if (_currentObject != null)
                ClearObject();

            _currentObject = obj;
            _originalLayers.Add(obj, obj.layer);

            Transform objTransform = obj.transform;
            int childCount = objTransform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject child = objTransform.GetChild(i).gameObject;

                _originalLayers.Add(child, child.layer);
            }
        }

        public void SwitchLayer(LayerMask mask) => SwitchLayer(mask.ToLayer());

        public void SwitchLayer(int layer)
        {
            _currentObject.layer = layer;

            Transform objTransform = _currentObject.transform;
            int childCount = objTransform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject child = objTransform.GetChild(i).gameObject;
                child.layer = layer;
            }
        }

        public void RestoreObjectLayers()
        {
            GameObject[] objects = _originalLayers.Keys.ToArray();
            int[] layers = _originalLayers.Values.ToArray();

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].layer = layers[i];
            }
        }

        public void ClearObject()
        {
            _currentObject = null;
            _originalLayers.Clear();
        }
    }
}