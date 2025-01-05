namespace MyCroBot.Strategies
{
    internal class BasicSettings
    {
        public string Instrument { get; set; }
        public decimal StartPrice { get; set; }
        public decimal TakeProfitPercents { get; set; }
        public decimal StopLossPercents { get; set; }
        public decimal InvestmentAmount { get; set; }
    }
}
