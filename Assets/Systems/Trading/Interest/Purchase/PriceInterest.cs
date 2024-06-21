using TradingSystem.Interest.Qualitative;

namespace TradingSystem.Interest
{
    public readonly struct PriceInterest
    {
        public readonly float basePrice;
        private readonly QualitativeInterest _qualitativeInterest;

        public PriceInterest(float basePrice, QualitativeInterest qualitativeInterest)
        {
            this.basePrice = basePrice;
            _qualitativeInterest = qualitativeInterest;
        }

        public float GetSellingPrice() => basePrice * _qualitativeInterest.interest;
        public float GetQualitativeInterest() => _qualitativeInterest.interest;

        override public string ToString()
        {
            return $"Base Price: {basePrice}, Qualitative Interest: {_qualitativeInterest}";
        }
    }
}