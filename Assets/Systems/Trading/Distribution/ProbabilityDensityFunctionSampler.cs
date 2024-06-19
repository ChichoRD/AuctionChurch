using System;
using UnityEngine;

namespace TradingSystem.Distribution
{
    /// <summary>
    /// Samples according to a density given by an animation curve.
    /// This assumes that the animation curve is non-negative everywhere.
    /// </summary>
    public readonly struct ProbabilityDensityFunctionSampler
    {
        private readonly AnimationCurve _densityCurve;
        private readonly IntegratedFunction _integratedDensity;

        public ProbabilityDensityFunctionSampler(AnimationCurve desityCurve, int integrationSteps)
        {
            _densityCurve = desityCurve;
            _integratedDensity = new IntegratedFunction(
                desityCurve.Evaluate,
                desityCurve.keys[0].time,
                desityCurve.keys[^1].time,
                integrationSteps);
        }

        /// <summary>
        /// Takes a value s in [0, 1], scales it up to the interval
        /// [0, totalIntegratedValue] and computes its inverse.
        /// </summary>
        public float InverseTransformSample(float probability)
        {
            probability *= _integratedDensity.Total;
            float lower = MinDistributionInput;
            float upper = MaxDistributionInput;
            const float PRECISION = 0.00001f;
            while (upper - lower > PRECISION)
            {
                float mid = (lower + upper) / 2f;
                float d = _integratedDensity.Evaluate(mid);
                if (d > probability)
                    upper = mid;
                else if (d < probability)
                    lower = mid;
                else
                    return mid;
            }

            return (lower + upper) / 2f;
        }

        public float ProbabilityOfNormalizedRange(float normalizedFrom, float normalizedTo) =>
            _integratedDensity.Range(
                Mathf.Lerp(MinDistributionInput, MaxDistributionInput, normalizedFrom),
                Mathf.Lerp(MinDistributionInput, MaxDistributionInput, normalizedTo)) 
            / _integratedDensity.Total;
        public float ProbabilityOfRange(float from, float to) => _integratedDensity.Range(from, to) / _integratedDensity.Total;

        public float RandomSample() => InverseTransformSample(UnityEngine.Random.value);

        private float MinDistributionInput => _densityCurve.keys[0].time;

        private float MaxDistributionInput => _densityCurve.keys[^1].time;

        [Serializable]
        public struct Builder
        {
            [SerializeField]
            private float _minValue;

            [SerializeField]
            private float _maxValue;

            [SerializeField]
            private AnimationCurve _probabilityDensityCurve;

            [SerializeField]
            [Min(0)]
            private int _integrationSteps;

            public Builder(float minValue, float maxValue, AnimationCurve probabilityDensityCurve, int integrationSteps)
            {
                _minValue = minValue;
                _maxValue = maxValue;
                _probabilityDensityCurve = probabilityDensityCurve;
                _integrationSteps = integrationSteps;
            }

            public readonly ProbabilityDensityFunctionSampler Build() =>
                new ProbabilityDensityFunctionSampler(_probabilityDensityCurve, _integrationSteps);
        }
    }
}