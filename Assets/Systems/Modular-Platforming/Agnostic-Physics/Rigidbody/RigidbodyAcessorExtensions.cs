using UnityEngine;

namespace AgnosticPhysics.Rigidbody
{
    public static class RigidbodyExtensions
    {
        // m * dv = F * dt
        // a = F / m
        // F = m * a
        public static void AddAcceleration(this IRigidbody rigidbody, Vector3 acceleration)
        {
            rigidbody.AddForce(acceleration * rigidbody.Mass);
        }

        // m * dv = F * dt
        // I = F * dt
        // F = I / dt
        public static void AddImpulse(this IRigidbody rigidbody, Vector3 impulse)
        {
            rigidbody.AddForce(impulse / Time.fixedDeltaTime);
        }

        // m * dv = F * dt
        // dv = F * dt / m
        // F = m * dv / dt
        public static void AddVelocity(this IRigidbody rigidbody, Vector3 velocity)
        {
            rigidbody.AddForce(rigidbody.Mass * velocity / Time.fixedDeltaTime);
        }

        public static RigidbodyConstraints FromRigidbodyConstraints2D(this RigidbodyConstraints2D constraints)
        {
            RigidbodyConstraints rigidbodyConstraints = RigidbodyConstraints.None;

            if ((constraints & RigidbodyConstraints2D.FreezePositionX) != 0)
                rigidbodyConstraints |= RigidbodyConstraints.FreezePositionX;
            if ((constraints & RigidbodyConstraints2D.FreezePositionY) != 0)
                rigidbodyConstraints |= RigidbodyConstraints.FreezePositionY;
            rigidbodyConstraints |= RigidbodyConstraints.FreezePositionZ;

            rigidbodyConstraints |= RigidbodyConstraints.FreezeRotationX;
            rigidbodyConstraints |= RigidbodyConstraints.FreezeRotationY;
            if ((constraints & RigidbodyConstraints2D.FreezeRotation) != 0)
                rigidbodyConstraints |= RigidbodyConstraints.FreezeRotationZ;

            return rigidbodyConstraints;
        }

        public static RigidbodyConstraints2D FromRigidbodyContraints3D(this RigidbodyConstraints constraints)
        {
            RigidbodyConstraints2D rigidbodyConstraints = RigidbodyConstraints2D.None;

            if ((constraints & RigidbodyConstraints.FreezePositionX) != 0)
                rigidbodyConstraints |= RigidbodyConstraints2D.FreezePositionX;
            if ((constraints & RigidbodyConstraints.FreezePositionY) != 0)
                rigidbodyConstraints |= RigidbodyConstraints2D.FreezePositionY;

            if ((constraints & RigidbodyConstraints.FreezeRotationZ) != 0)
                rigidbodyConstraints |= RigidbodyConstraints2D.FreezeRotation;

            return rigidbodyConstraints;
        }
    }
}