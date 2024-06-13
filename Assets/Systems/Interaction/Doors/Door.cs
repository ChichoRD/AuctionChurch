using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuctionChurch.Interaction.Doors
{
    public class Door : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private Transform _anchor;
        [SerializeField] private Collider[] _doorColliders;

        [Header("Positioning")]
        [SerializeField] private float _openingTime = 0.5f;
        [SerializeField] private Vector3 _openRotation;
        private Vector3 _closedRotation;
        private bool _isOpen = false;
        private bool _isOpening = false;

        private void Awake()
        {
            _closedRotation = _anchor.localRotation.eulerAngles;
        }

        public void Accept(IInteractor interactor) => interactor.Interact(this);

        public void ToggleOpen()
        {
            if (_isOpening)
                return;

            Vector3 targetRot = _isOpen ? _closedRotation : _openRotation;
            StartCoroutine(IELerpPositionAndRotation(Quaternion.Euler(targetRot)));

            _isOpen = !_isOpen;
        }

        private IEnumerator IELerpPositionAndRotation(Quaternion targetRot)
        {
            _isOpening = true;
            ToggleColliders(false);

            float elapsedTime = 0;
            Quaternion startRot = _anchor.localRotation;

            while (elapsedTime < _openingTime)
            {
                float percent = elapsedTime / _openingTime;

                _anchor.localRotation = Quaternion.Slerp(startRot, targetRot, percent);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _isOpening = false;
            ToggleColliders(true);
        }

        private void ToggleColliders(bool enabled)
        {
            for (int i = 0; i < _doorColliders.Length; i++)
                _doorColliders[i].enabled = enabled;
        }
    }
}