using UnityEngine;

namespace TradingSystem.Interest.Quantitative
{
    [CreateAssetMenu(fileName = "New Quantitative Interest", menuName = "Trading System/Interest/Quantitative Interest Flyweight")]
    internal class QuantitativeInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private QuantitativeInterestParameters.Builder _interestParameters;

        public QuantitativeInterest Create()
        {
            QuantitativeInterestParameters parameters = _interestParameters.Build();
            float[] cumulativeInterests = new float[parameters.maximumQuantity - parameters.minimumQuantity + 1];
            float cummulated = 0.0f;
            for (int i = 0; i < cumulativeInterests.Length; i++)
            {
                float t = Mathf.InverseLerp(parameters.minimumQuantity, parameters.maximumQuantity, parameters.minimumQuantity + i);

                cummulated += parameters.interestCurve.Evaluate(t);
                cumulativeInterests[i] = cummulated;
            }

            return new QuantitativeInterest(parameters.minimumQuantity, cumulativeInterests);
        }
    }
}