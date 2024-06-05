namespace InputBox.Writeable
{
    public interface IInputWriteable<in TInput>
    {
        bool TrySetInput(TInput input);
    }
}