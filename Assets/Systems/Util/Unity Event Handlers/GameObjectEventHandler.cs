using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void ToggleActive()
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }
}
