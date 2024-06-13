using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction.Holding
{
    public class ObjectPlacerListener : MonoBehaviour
    {
        [SerializeField] private ObjectPlacer _placer;

        [Header("OnPlace Events")]
        [SerializeField] private UnityEvent _onPlaceEvents;

        [Header("OnDrop Events")]
        [SerializeField] private UnityEvent _onDropEvents;

        private void OnEnable()
        {
            _placer.OnPlace += _onPlaceEvents.Invoke;
            _placer.OnDrop += _onDropEvents.Invoke;
        }

        private void OnDisable()
        {
            _placer.OnPlace -= _onPlaceEvents.Invoke;
            _placer.OnDrop -= _onDropEvents.Invoke;
        }
    }
}