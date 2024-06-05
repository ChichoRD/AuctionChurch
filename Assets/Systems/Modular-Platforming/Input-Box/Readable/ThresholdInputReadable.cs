using UnityEngine;

namespace InputBox.Readable
{
    internal class ThresholdInputReadable : MonoBehaviour,
        IInputReadable<float>, IInputReadable<Vector2>, IInputReadable<Vector3>, IInputReadable<Vector4>
    {
        [SerializeField]
        [Min(0.0f)]
        private float _threshold = Mathf.Epsilon;
        private float SquaredThreshold => _threshold * _threshold;

        [SerializeField]
        private Vector4 _defaultInput = Vector3.right;

        private Vector4 _lastValidInput;

        [SerializeReference]
        private Object _inputReadableObject;

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

        public bool AboveThreshold(Vector4 input) => input.sqrMagnitude > SquaredThreshold;
        public bool AboveThreshold(Vector3 input) => input.sqrMagnitude > SquaredThreshold;
        public bool AboveThreshold(Vector2 input) => input.sqrMagnitude > SquaredThreshold;
        public bool AboveThreshold(float input) => input > _threshold;

        public float GetInput()
        {
            float input = _inputReadableFloat.GetInput();
            return _lastValidInput.x = AboveThreshold(input)
                   ? input
                   : AboveThreshold(_lastValidInput.x)
                     ? _lastValidInput.x
                     : _defaultInput.x;
        }

        Vector2 IInputReadable<Vector2>.GetInput()
        {
            Vector2 input = _inputReadableVector2.GetInput();
            return _lastValidInput = AboveThreshold(input)
                   ? input
                   : AboveThreshold(_lastValidInput)
                     ? _lastValidInput
                     : _defaultInput;
        }

        Vector3 IInputReadable<Vector3>.GetInput()
        {
            Vector3 input = _inputReadableVector3.GetInput();
            return _lastValidInput = AboveThreshold(input)
                   ? input
                   : AboveThreshold(_lastValidInput)
                     ? _lastValidInput
                     : _defaultInput;
        }

        Vector4 IInputReadable<Vector4>.GetInput()
        {
            Vector4 input = _inputReadableVector4.GetInput();
            return _lastValidInput = AboveThreshold(input)
                   ? input
                   : AboveThreshold(_lastValidInput)
                     ? _lastValidInput
                     : _defaultInput;
        }
    }
}