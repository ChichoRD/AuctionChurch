using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.Physics
{
    public static class PhysicsHelper
    {
        public static int ToLayer(this LayerMask mask)
        {
            int result = mask > 0 ? 0 : 31;

            while (mask > 1)
            {
                mask >>= 1;
                result++;
            }

            return result;
        }
    }
}