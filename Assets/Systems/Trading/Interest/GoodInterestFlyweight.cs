using System;
using TradingSystem.Interest.Qualitative;
using TradingSystem.Interest.Quantitative;
using UnityEngine;

namespace TradingSystem.Interest
{
    [Serializable]
    internal struct GoodInterestBuilder
    {
        [SerializeField]
        private Good _good;

        [SerializeField]
        private QualitativeInterestFlyweight _qualitativeInterest;

        [SerializeField]
        private QuantitativeInterestFlyweight _quantitativeInterest;

        public GoodInterestBuilder(Good good, QualitativeInterestFlyweight qualitativeInterest, QuantitativeInterestFlyweight quantitativeInterest)
        {
            _good = good;
            _qualitativeInterest = qualitativeInterest;
            _quantitativeInterest = quantitativeInterest;
        }

        public readonly GoodInterest Build() =>
            new GoodInterest(_good, _qualitativeInterest.Create(), _quantitativeInterest.Create());
    }
}