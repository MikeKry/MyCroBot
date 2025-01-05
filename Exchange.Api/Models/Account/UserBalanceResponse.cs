namespace Exchange.Api.Models.Account
{
    public class UserBalanceResponse : ResponseModel<UserBalanceResult>
    {
        public override string Method { get => "private/user-balance"; set => base.Method = value; }
    }

    public class UserBalanceResult
    {
        public Data[] data { get; set; }
    }

    public class Data
    {
        public string total_available_balance { get; set; }
        public string total_margin_balance { get; set; }
        public string total_initial_margin { get; set; }
        public string total_position_im { get; set; }
        public string total_haircut { get; set; }
        public string total_maintenance_margin { get; set; }
        public string total_position_cost { get; set; }
        public string total_cash_balance { get; set; }
        public string total_collateral_value { get; set; }
        public string total_session_unrealized_pnl { get; set; }
        public string instrument_name { get; set; }
        public string total_session_realized_pnl { get; set; }
        public bool is_liquidating { get; set; }
        public string total_effective_leverage { get; set; }
        public string position_limit { get; set; }
        public string used_position_limit { get; set; }
        public Position_Balances[] position_balances { get; set; }
    }

    public class Position_Balances
    {
        public string instrument_name { get; set; }
        public string quantity { get; set; }
        public string market_value { get; set; }
        public string collateral_eligible { get; set; }
        public string haircut { get; set; }
        public string collateral_amount { get; set; }
        public string max_withdrawal_balance { get; set; }
        public string reserved_qty { get; set; }
    }
}