using System;

namespace ModularPlatforming.Movement.Applier
{
    public interface IObservableMovementApplier
    {
        bool InMotion { get; }
        event EventHandler MotionStarted;
        event EventHandler MotionEnded;
    }
}
