using UnityEngine;

public class CameraTransformConstraint : MonoBehaviour
{
    private Transform parent;

    private void Awake()
    {
        parent = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(parent.position, parent.rotation);
    }
}