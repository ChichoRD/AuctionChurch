using UnityEngine;

namespace TradingSystem.Interest.Quantitative
{
    public readonly struct QuantitativeInterest
    {
        public readonly int minimumQuantity;
        public readonly float[] cumulativeInterests;

        public QuantitativeInterest(int minimumQuantity, float[] cumulativeInterests)
        {
            this.minimumQuantity = minimumQuantity;
            this.cumulativeInterests = cumulativeInterests;
        }

        public int GetRandomItemQuantity()
        {
            float randomValue = UnityEngine.Random.Range(0, cumulativeInterests[^1]);
            int startIndex = 0;
            int endIndex = cumulativeInterests.Length;

            while (startIndex < endIndex)
            {
                int middleIndex = (startIndex + endIndex) / 2;

                if (cumulativeInterests[middleIndex] < randomValue)
                    startIndex = middleIndex + 1;
                else
                    endIndex = middleIndex;
            }

            return minimumQuantity + startIndex;
        }

        public float GetQuantityRangeInterest(int minimumQuantity, int maximumQuantity)
        {
            int minIndex = Mathf.Max(minimumQuantity - this.minimumQuantity, 0);
            int maxIndex = Mathf.Min(maximumQuantity - this.minimumQuantity, cumulativeInterests.Length - 1);

            return cumulativeInterests[maxIndex] - cumulativeInterests[minIndex];
        }
    }
}