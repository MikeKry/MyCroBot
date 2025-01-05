namespace Exchange.Api.Models
{
    public record struct BalanceRecord(decimal balance, decimal buyPrice, string instrument)
    {
        public static implicit operator (decimal balance, decimal buyPrice, string buyInstrument)(BalanceRecord value)
        {
            return (value.balance, value.buyPrice, value.instrument);
        }

        public static implicit operator BalanceRecord((decimal balance, decimal buyPrice, string buyInstrument) value)
        {
            return new BalanceRecord(value.balance, value.buyPrice, value.buyInstrument);
        }
    }
}
