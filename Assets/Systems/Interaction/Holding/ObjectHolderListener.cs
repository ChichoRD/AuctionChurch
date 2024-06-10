using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectHolderListener : MonoBehaviour
    {
        [SerializeField] private ObjectHolder _holder;

        [Header("OnHold Events")]
        [SerializeField] private UnityEvent<HoldableObject> _onHold;
        [SerializeField] private UnityEvent<GameObject> _gameObjectOnHold;

        [Header("OnRelease Events")]
        [SerializeField] private UnityEvent<HoldableObject> _onRelease;
        [SerializeField] private UnityEvent<GameObject> _gameObjectOnRelease;

        private void OnEnable()
        {
            _holder.OnHold += OnHold;
            _holder.OnRelease += OnRelease;
        }

        private void OnDisable()
        {
            _holder.OnHold -= OnHold;
            _holder.OnRelease -= OnRelease;
        }

        private void OnHold(HoldableObject obj)
        {
            _onHold.Invoke(obj);
            _gameObjectOnHold.Invoke(obj.gameObject);
        }

        private void OnRelease(HoldableObject obj)
        {
            _onRelease.Invoke(obj);
            _gameObjectOnRelease.Invoke(obj.gameObject);
        }
    }
}