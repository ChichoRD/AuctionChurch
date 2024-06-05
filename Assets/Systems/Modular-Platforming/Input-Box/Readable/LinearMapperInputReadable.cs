using UnityEngine;

namespace InputBox.Readable
{

    internal class LinearMapperInputReadable : MonoBehaviour,
        IInputReadable<float>, IInputReadable<Vector2>, IInputReadable<Vector3>, IInputReadable<Vector4>
    {
        [SerializeReference]
        private Object _inputReadableObject;

        [SerializeField]
        private Matrix4x4 _transformation = Matrix4x4.identity;

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

        public float GetInput() => _transformation.MultiplyVector(new Vector3(_inputReadableFloat.GetInput(), 0.0f, 0.0f)).x;

        Vector2 IInputReadable<Vector2>.GetInput() => _transformation.MultiplyVector(_inputReadableVector2.GetInput());

        Vector3 IInputReadable<Vector3>.GetInput() => _transformation.MultiplyVector(_inputReadableVector3.GetInput());

        Vector4 IInputReadable<Vector4>.GetInput() => _transformation.MultiplyVector(_inputReadableVector4.GetInput());
    }
}