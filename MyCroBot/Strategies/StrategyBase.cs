namespace MyCroBot.Strategies
{
    internal class StrategyBase
    {
        protected BasicSettings _basicSettings;

        public StrategyBase(BasicSettings basicSettings)
        {
            _basicSettings = basicSettings;
        }

        public bool Confirm(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine() == "y";
        }

        public BasicSettings GetBasicSettings()
        {
            return _basicSettings;
        }

        /// <summary>
        /// Rounded to instrumentQuantityDecimalPlaces.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="investmentAmount"></param>
        /// <returns></returns>
        internal decimal GetTradeAmountFromPrice(decimal price, decimal investmentAmount, int instrumentQuantityDecimalPlaces = 0)
        {
            return Math.Round(investmentAmount / price, instrumentQuantityDecimalPlaces);
        }
    }
}