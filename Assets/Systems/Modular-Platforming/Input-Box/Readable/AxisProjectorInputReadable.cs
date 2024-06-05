using UnityEngine;

namespace InputBox.Readable
{
    internal class AxisProjectorInputReadable : MonoBehaviour,
        IInputReadable<float>, IInputReadable<Vector2>, IInputReadable<Vector3>, IInputReadable<Vector4>
    {
        [SerializeReference]
        private Object _inputReadableObject;

        [field: SerializeField]
        public Vector4 ProjectionAxis { get; set; } = Vector3.up;
        [SerializeField] private bool _renormalize = true;

        private IInputReadable<float> _inputReadableFloat;
        private IInputReadable<Vector2> _inputReadableVector2;
        private IInputReadable<Vector3> _inputReadableVector3;
        private IInputReadable<Vector4> _inputReadableVector4;

        private void Awake()
        {
            _inputReadableFloat = _inputReadableObject as IInputReadable<float>;
            _inputReadableVector2 = _inputReadableObject as IInputReadable<Vector2>;
            _inputReadableVector3 = _inputReadableObject as IInputReadable<Vector3>;
            _inputReadableVector4 = _inputReadableObject as IInputReadable<Vector4>;
        }

        public float GetInput()
        {
            Vector3 projectedInput = Vector3.Project(new Vector3(_inputReadableFloat.GetInput(), 0.0f, 0.0f), ProjectionAxis);
            return _renormalize ? Mathf.Sign(projectedInput.x) : projectedInput.x;
        }

        Vector2 IInputReadable<Vector2>.GetInput()
        {
            Vector2 projectedInput = Vector3.Project(_inputReadableVector2.GetInput(), ProjectionAxis);
            return _renormalize ? projectedInput.normalized : projectedInput;
        }

        Vector3 IInputReadable<Vector3>.GetInput()
        {
            Vector3 projectedInput = Vector3.Project(_inputReadableVector3.GetInput(), ProjectionAxis);
            return _renormalize ? projectedInput.normalized : projectedInput;
        }

        Vector4 IInputReadable<Vector4>.GetInput()
        {
            Vector4 projectedInput = Vector4.Project(_inputReadableVector4.GetInput(), ProjectionAxis);
            return _renormalize ? projectedInput.normalized : projectedInput;
        }
    }
}