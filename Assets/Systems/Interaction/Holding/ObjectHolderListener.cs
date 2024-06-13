using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectHolderListener : MonoBehaviour
    {
        [SerializeField] private ObjectHolder _holder;

        [Header("OnHold Events")]
        [SerializeField] private UnityEvent<GameObject> _gameObjectOnHold;

        [Header("OnRelease Events")]
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

        private void OnHold(HoldableObject obj) => _gameObjectOnHold.Invoke(obj.gameObject);

        private void OnRelease(HoldableObject obj) => _gameObjectOnRelease.Invoke(obj.gameObject);
    }
}