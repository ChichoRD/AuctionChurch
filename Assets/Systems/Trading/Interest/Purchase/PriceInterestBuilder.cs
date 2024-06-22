using System;
using TradingSystem.Interest.Qualitative;
using UnityEngine;

namespace TradingSystem.Interest
{
    [Serializable]
    internal struct PriceInterestBuilder
    {
        [SerializeField]
        [Min(0.0f)]
        private float _basePrice;

        [SerializeField]
        private QualitativeInterestFlyweight _qualitativeInterest;

        public PriceInterestBuilder(float price, QualitativeInterestFlyweight qualitativeInterest)
        {
            _basePrice = price;
            _qualitativeInterest = qualitativeInterest;
        }

        public readonly PriceInterest Build() =>
            new PriceInterest(_basePrice, _qualitativeInterest.Create());
    }
}