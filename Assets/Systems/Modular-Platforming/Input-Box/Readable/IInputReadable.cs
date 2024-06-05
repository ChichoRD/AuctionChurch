namespace InputBox.Readable
{
    public interface IInputReadable<out TInput>
    {
        TInput GetInput();
    }
}
