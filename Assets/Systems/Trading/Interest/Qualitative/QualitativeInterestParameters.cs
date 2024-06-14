using System;
using UnityEngine;

namespace TradingSystem.Interest.Qualitative
{
    public readonly struct QualitativeInterestParameters
    {
        public readonly AnimationCurve interest;

        public QualitativeInterestParameters(AnimationCurve interest)
        {
            this.interest = interest;
        }

        [Serializable]
        public struct Builder
        {
            public AnimationCurve interests;

            public readonly QualitativeInterestParameters Build() =>
                new QualitativeInterestParameters(interests);
        }
    }
}