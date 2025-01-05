namespace Exchange.Api.Models.Trading
{
    public class GetOpenOrderResponse : ResponseModel<OpenOrderResponse>
    {
        public override string Method { get => "private/get-open-orders"; set => base.Method = value; }
    }

    public class OpenOrderResponse
    {
        public OpenOrderData[] data { get; set; }
    }

    public class OpenOrderData
    {
        public string account_id { get; set; }
        public string order_id { get; set; }
        public string client_oid { get; set; }
        public string order_type { get; set; }
        public string time_in_force { get; set; }
        public string side { get; set; }
        public object[] exec_inst { get; set; }
        public string quantity { get; set; }
        public string limit_price { get; set; }
        public string order_value { get; set; }
        public string maker_fee_rate { get; set; }
        public string taker_fee_rate { get; set; }
        public string avg_price { get; set; }
        public string cumulative_quantity { get; set; }
        public string cumulative_value { get; set; }
        public string cumulative_fee { get; set; }
        public string status { get; set; }
        public string update_user_id { get; set; }
        public string order_date { get; set; }
        public string instrument_name { get; set; }
        public string fee_instrument_name { get; set; }
        public long create_time { get; set; }
        public string create_time_ns { get; set; }
        public long update_time { get; set; }
    }
}