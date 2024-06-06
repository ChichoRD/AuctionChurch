using UnityEngine;
using UnityEngine.Events;

namespace AuctionChurch.Interaction
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public abstract void Interact();
    }
}