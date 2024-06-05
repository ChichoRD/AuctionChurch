using UnityEngine.Events;

namespace InputBox.Observable
{
    public interface IObservableInput
    {
        UnityEvent InputReceived { get; }
        UnityEvent InputExpired { get; }
    }
}