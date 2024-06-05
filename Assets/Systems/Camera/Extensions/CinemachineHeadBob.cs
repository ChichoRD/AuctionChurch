using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;

public class CinemachineHeadBob : CinemachineExtension
{
    [Header("General")]
    [SerializeField] private CinemachineCore.Stage _applyAfter = CinemachineCore.Stage.Aim;
    [SerializeField] private bool _preserveComposition;
    [SerializeField] private NoiseSettings _noiseSettings;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _velocityProjector = new Vector3(1.0f, 0.0f, 1.0f);

    [Header("Axes Amplitudes")]
    [SerializeField] private Vector3 _noisePositionAxesFactor = Vector3.up;
    [SerializeField] private Vector3 _noiseRotationAxesFactor = Vector3.forward;

    [Header("Global Amplitudes")]
    [SerializeField] private float _noisePositionAmplitudeFactor = 0.5f;
    [SerializeField] private float _noiseRotationAmplitudeFactor = 0.3f;

    [Header("Frequency")]
    [SerializeField] private float _noiseFrequencyFactor = 0.3f;

    private void Start() => enabled = _rigidbody != null && _noiseSettings != null;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != _applyAfter) return;

        GetRawOffsets(out Vector3 offset, out Quaternion rotation);
        Vector3 positionCorrectionContribution = state.RawOrientation * offset;
        Vector3 rotationCorrectionContribution = Vector3.Scale(rotation.eulerAngles, _noiseRotationAxesFactor);

        ApplyRotationOffset(ref state, rotationCorrectionContribution);
        ApplyPositionOffset(stage, ref state, positionCorrectionContribution);
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

    private void ApplyPositionOffset(CinemachineCore.Stage stage, ref CameraState state, Vector3 positionCorrectionContribution)
    {
        bool preserveAim = _preserveComposition && state.HasLookAt && stage > CinemachineCore.Stage.Body;
        Vector3 screenOffset = Vector2.zero;
        if (preserveAim)
        {
            screenOffset = state.RawOrientation.GetCameraRotationToTarget(
                state.ReferenceLookAt - state.CorrectedPosition, state.ReferenceUp);
        }

        state.PositionCorrection += positionCorrectionContribution;

        if (preserveAim)
        {
            var q = Quaternion.LookRotation(
                state.ReferenceLookAt - state.CorrectedPosition, state.ReferenceUp);
            q = q.ApplyCameraRotation(-screenOffset, state.ReferenceUp);
            state.RawOrientation = q;

            return;
        }

        state.ReferenceLookAt += positionCorrectionContribution;
    }

    private void GetRawOffsets(out Vector3 offset, out Quaternion rotation)
    {
        Vector3 alignment = Vector3.Scale(_rigidbody.velocity, _velocityProjector);
        float alignedSpeed = Mathf.Sqrt(Vector3.Dot(alignment, alignment));
        float positionAmplitude = alignedSpeed * _noisePositionAmplitudeFactor;
        float rotationAmplitude = alignedSpeed * _noiseRotationAmplitudeFactor;

        float phase = _noiseFrequencyFactor * Time.realtimeSinceStartup;
        _noiseSettings.GetSignal(phase, out offset, out rotation);

        offset = Vector3.Scale(offset, _noisePositionAxesFactor) * positionAmplitude;
        rotation = Quaternion.SlerpUnclamped(Quaternion.identity, rotation, rotationAmplitude);
    }
}
