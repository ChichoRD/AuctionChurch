using UnityEngine;

namespace ModularPlatforming.Movement.Magnitude
{
    internal readonly struct Velocity
    {
        public Vector3 Value { get; }

        public Velocity(Vector3 value)
        {
            Value = value;
        }

        public static implicit operator Vector3(Velocity velocity) => velocity.Value;
        public static implicit operator Velocity(Vector3 velocity) => new Velocity(velocity);
    }
}