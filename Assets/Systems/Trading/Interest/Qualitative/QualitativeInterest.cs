namespace TradingSystem.Interest.Qualitative
{
    public readonly struct QualitativeInterest
    {
        public readonly float interest;

        public QualitativeInterest(float interest)
        {
            this.interest = interest;
        }

        public override string ToString()
        {
            return $"Interest: {interest}";
        }
    }
}