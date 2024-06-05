using InputBox.Observable;
using InputBox.Readable;
using UnityEngine;
using UnityEngine.Events;

namespace InputBox.Writeable
{
    internal class VectorInputService : MonoBehaviour,
        IInputWriteable<float>, IInputWriteable<Vector2>, IInputWriteable<Vector3>, IInputWriteable<Vector4>,
        IInputReadable<float>, IInputReadable<Vector2>, IInputReadable<Vector3>, IInputReadable<Vector4>,
        IObservableInput
    {
        private Vector4 _input;
        public Vector4 Input
        {
            get => _input;
            set
            {
                _input = value;
                InputReceived?.Invoke();
            }
        }

        [field: SerializeField]
        public UnityEvent InputReceived { get; private set; }

        [field: SerializeField]
        public UnityEvent InputExpired { get; private set; }

        public float GetInput() => Input.x;
        Vector2 IInputReadable<Vector2>.GetInput() => Input;
        Vector3 IInputReadable<Vector3>.GetInput() => Input;
        Vector4 IInputReadable<Vector4>.GetInput() => Input;

        public bool TrySetInput(float input)
        {
            Input = new Vector4(input, Input.y, Input.z, Input.w);
            return true;
        }

        public bool TrySetInput(Vector2 input)
        {
            Input = input;
            return true;
        }

        public bool TrySetInput(Vector3 input)
        {
            Input = input;
            return true;
        }

        public bool TrySetInput(Vector4 input)
        {
            Input = input;
            return true;
        }
    }
}