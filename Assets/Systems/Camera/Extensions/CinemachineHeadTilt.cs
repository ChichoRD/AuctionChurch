using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CinemachineHeadTilt : CinemachineExtension
{
    [Header("General")]
    [SerializeField] private CinemachineCore.Stage _applyAfter = CinemachineCore.Stage.Aim;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _relativeTransform;
    [SerializeField] private Vector3 _velocityProjector = new Vector3(1.0f, 1.0f, 0.5f);

    [Header("Axis Amplitude")]
    [SerializeField] private Vector3 _tiltAxesFactor = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] private Vector3 _tiltAxesMaxAmplitude = new Vector3(2.0f, 0.0f, 2.0f);

    [Header("Global Amplitude")]
    [SerializeField] private float _tiltAmplitudeFactor = 1.0f;

    private const int VELOCITY_QUEUE_SIZE = 20;
    private readonly Queue<Vector3> _velocityQueue = new Queue<Vector3>(VELOCITY_QUEUE_SIZE);

    private void Start() => enabled = _rigidbody != null && _relativeTransform != null;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != _applyAfter) return;

        Vector3 averageVelocity = GetAverageVelocity();

        Vector3 projectedVelocity = GetRelativeProjectedVelocity(averageVelocity);

        Vector3 angles = GetVelocitySwayRotation(projectedVelocity);

        ApplyRotationOffset(ref state, angles);
    }

    private Vector3 GetRelativeProjectedVelocity(Vector3 averageVelocity)
    {
        Vector3 relativeVelocity = _relativeTransform.InverseTransformDirection(averageVelocity);
        Vector3 projectedVelocity = Vector3.Scale(relativeVelocity, _velocityProjector) * _tiltAmplitudeFactor;
        return projectedVelocity;
    }

    private Vector3 GetAverageVelocity()
    {
        while (_velocityQueue.Count >= VELOCITY_QUEUE_SIZE) _velocityQueue.Dequeue();
        _velocityQueue.Enqueue(_rigidbody.velocity);

        float weight = 1.0f;
        float accumulatedWeight = 1.0f;
        float dw = 1 / (float)_velocityQueue.Count;
        Vector3 averageVelocity = _velocityQueue.Aggregate(Vector3.zero, (acc, v) =>
        {
            weight += dw;
            accumulatedWeight += weight;

            return acc + v * weight;
        }) / accumulatedWeight;
        return averageVelocity;
    }

    private Vector3 GetVelocitySwayRotation(Vector3 projectedVelocity)
    {
        var rotationX = Quaternion.AngleAxis(projectedVelocity.x, Vector3.up);
        var rotationY = Quaternion.identity; //Quaternion.AngleAxis(0.0f, Vector3.right);

        var rotationZ = Quaternion.AngleAxis(-projectedVelocity.x, Vector3.forward);
        var rotationY2 = Quaternion.AngleAxis(projectedVelocity.y, Vector3.right);
        var rotationY3 = Quaternion.AngleAxis(projectedVelocity.z, Vector3.right);
        var quatTargetRotation = rotationZ * rotationX * rotationY * rotationY2 * rotationY3;
        var angles = GetSignedEulerAngles(Vector3.Scale(quatTargetRotation.eulerAngles, _tiltAxesFactor));

        angles.x = Mathf.Clamp(angles.x, -_tiltAxesMaxAmplitude.x, _tiltAxesMaxAmplitude.x);
        angles.y = Mathf.Clamp(angles.y, -_tiltAxesMaxAmplitude.y, _tiltAxesMaxAmplitude.y);
        angles.z = Mathf.Clamp(angles.z, -_tiltAxesMaxAmplitude.z, _tiltAxesMaxAmplitude.z);
        return angles;
    }

    private void ApplyRotationOffset(ref CameraState state, Vector3 rotation)
    {
        var lens = state.Lens;

        var (tilt, pan, dutch) = (rotation.x, rotation.y, rotation.z);

        // Tilt by local X
        var qTilted = state.RawOrientation * Quaternion.AngleAxis(tilt, Vector3.right);
        // Pan in world space
        var qDesired = Quaternion.AngleAxis(pan, state.ReferenceUp) * qTilted;
        state.OrientationCorrection = Quaternion.Inverse(state.CorrectedOrientation) * qDesired;
        // And dutch at the end
        lens.Dutch += dutch;
        // Finally zoom
        state.Lens = lens;
    }

    private static Vector3 GetSignedEulerAngles(Vector3 angles)
    {
        Vector3 signedAngles = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            signedAngles[i] = (angles[i] + 180f) % 360f - 180f;
        }
        return signedAngles;
    }
}