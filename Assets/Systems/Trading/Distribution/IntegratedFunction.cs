using System;
using UnityEngine;

namespace TradingSystem.Distribution
{
    /// <summary>
    /// Provides a numerically integrated version of a function.
    /// </summary>
    public readonly struct IntegratedFunction
    {
        private readonly float[] _cumulated;
        private readonly float _from, _to;

        /// <summary>
        /// Integrates a function on an interval. Use the steps parameter to control
        /// the precision of the numerical integration. Larger step values lead to
        /// better precision.
        /// </summary>
        public IntegratedFunction(Func<float, float> function, float from, float to, int steps)
        {
            _from = from;
            _to = to;
            _cumulated = Integrate(function, from, to, steps + 1);
        }

        private static float[] Integrate(Func<float, float> function, float from, float to, int steps)
        {
            float[] cumulated = new float[steps];
            float segment = (to - from) / (steps - 1);
            float lastY = function(from);
            float sum = 0;
            cumulated[0] = 0;
            for (int i = 1; i < steps; i++)
            {
                float x = from + i * segment;
                float nextY = function(x);
                sum += segment * (nextY + lastY) / 2;
                lastY = nextY;
                cumulated[i] = sum;
            }

            return cumulated;
        }

        /// <summary>
        /// Evaluates the integrated function at any point in the interval.
        /// </summary>
        public float Evaluate(float x)
        {
            Debug.Assert(_from <= x && x <= _to);
            float t = Mathf.InverseLerp(_from, _to, x);
            int lower = (int)(t * _cumulated.Length);
            int upper = (int)(t * _cumulated.Length + .5f);
            if (lower == upper || upper >= _cumulated.Length)
                return _cumulated[lower];
            float innerT = Mathf.InverseLerp(lower, upper, t * _cumulated.Length);
            return (1 - innerT) * _cumulated[lower] + innerT * _cumulated[upper];
        }

        public float Range(float from, float to) => Evaluate(to) - Evaluate(from);

        /// <summary>
        /// Returns the total value integrated over the whole interval.
        /// </summary>
        public float Total => _cumulated[^1];
    }
}