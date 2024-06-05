using UnityEngine;

namespace ModularPlatforming.Movement.Magnitude
{
    internal readonly struct Acceleration
    {
        public Vector3 Value { get; }

        public Acceleration(Vector3 value)
        {
            Value = value;
        }

        public static implicit operator Vector3(Acceleration acceleration) => acceleration.Value;
        public static implicit operator Acceleration(Vector3 acceleration) => new Acceleration(acceleration);
    }
}