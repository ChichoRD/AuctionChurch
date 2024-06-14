using System;
using UnityEngine;

namespace TradingSystem.Interest.Quantitative
{
    public readonly struct QuantitativeInterestParameters
    {
        public readonly int minimumQuantity;
        public readonly int maximumQuantity;
        public readonly AnimationCurve interestCurve;

        public QuantitativeInterestParameters(int minimumQuantity, int maximumQuantity, AnimationCurve interestCurve)
        {
            this.minimumQuantity = minimumQuantity;
            this.maximumQuantity = maximumQuantity;
            this.interestCurve = interestCurve;
        }

        [Serializable]
        public struct Builder
        {
            [Min(0)]
            public int minimumQuantity;
            [Min(0)]
            public int maximumQuantity;
            public AnimationCurve interestCurve;

            public Builder(int minimumQuantity, int maximumQuantity, AnimationCurve interestCurve)
            {
                this.minimumQuantity = minimumQuantity;
                this.maximumQuantity = maximumQuantity;
                this.interestCurve = interestCurve;
            }

            public readonly QuantitativeInterestParameters Build() =>
                new QuantitativeInterestParameters(minimumQuantity, maximumQuantity, interestCurve);
        }
    }
}