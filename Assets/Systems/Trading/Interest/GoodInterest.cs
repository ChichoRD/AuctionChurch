using TradingSystem.Interest.Qualitative;
using TradingSystem.Interest.Quantitative;

namespace TradingSystem.Interest
{
    public readonly struct GoodInterest
    {
        public readonly Good good;
        private readonly QualitativeInterest _qualitativeInterest;
        private readonly QuantitativeInterest _quantitativeInterest;

        public GoodInterest(Good good, QualitativeInterest qualitativeInterest, QuantitativeInterest quantitativeInterest)
        {
            this.good = good;
            _qualitativeInterest = qualitativeInterest;
            _quantitativeInterest = quantitativeInterest;
        }

        public float GetQualitativeInterest() => _qualitativeInterest.interest;
        public int GetRandomItemQuantity() => _quantitativeInterest.GetRandomItemQuantity();
        public float GetQuantityRangeInterest(int minimumQuantity, int maximumQuantity) =>
            _quantitativeInterest.GetQuantityRangeInterest(minimumQuantity, maximumQuantity);
    }
}