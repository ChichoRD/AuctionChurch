using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateController : MonoBehaviour
{
    [SerializeField] private CursorLockMode _cursorLockMode = CursorLockMode.Locked;
    [SerializeField] private bool _cursorVisibility = false;

    public CursorLockMode CursorLockMode { get => _cursorLockMode; set => UpdateLockMode(value); }
    public bool CursorVisibility { get => _cursorVisibility; set => UpdateVisibility(value); }

    private void OnEnable()
    {
        CursorLockMode = _cursorLockMode;
        CursorVisibility = _cursorVisibility;

        Cursor.lockState = _cursorLockMode;
        Cursor.visible = _cursorVisibility;
    }

    private void UpdateLockMode(CursorLockMode mode)
    {
        _cursorLockMode = mode;
        Cursor.lockState = _cursorLockMode;
    }

    private void UpdateVisibility(bool visible)
    {
        _cursorVisibility = visible;
        Cursor.visible = _cursorVisibility;
    }
}