using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(ParentConstraint))]
public class CameraTransformConstraint : MonoBehaviour
{
    private ParentConstraint _constraint;

    private void Awake()
    {
        _constraint = GetComponent<ParentConstraint>();

        ConstraintSource source = new()
        {
            sourceTransform = Camera.main.transform,
            weight = 1
        };
        _constraint.AddSource(source);
    }

    private void OnValidate()
    {
        _constraint = GetComponent<ParentConstraint>();

        if (_constraint.sourceCount > 0)
        {
            Debug.LogWarning("This ParentConstraint is controlled by the attached CameraTransformConstraint!");

            for (int i = 0; i < _constraint.sourceCount; i++)
                _constraint.RemoveSource(i);
        }
    }
}