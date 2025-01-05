namespace Exchange.Api.Models.Market
{
    public record CandlestickRecord
    {
        public bool IsUp { get; set; }
        public bool IsDown { get; set; }
        public decimal Price { get; set; }
        public decimal ChangePerc { get; set; }
    }
}
