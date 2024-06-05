namespace ModularPlatforming.Movement.Applier
{
    public interface ICancellableMovementApplier
    {
        bool TryCancel();
    }
}
