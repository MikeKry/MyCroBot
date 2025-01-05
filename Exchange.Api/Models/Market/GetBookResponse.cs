using RestSharp;

namespace Exchange.Api.Models.Market
{
    public class GetBookResponse : ResponseModel<BookResponse>
    {
        public override string Method { get => "public/get-book"; set => base.Method = value; }
    }

    public class BookResponse
    {
        public int depth { get; set; }
        public BookData[] data { get; set; }
        public string instrument_name { get; set; }
    }

    public class BookData
    {
        /// <summary>
        /// Bids array: [0] = Price, [1] = Quantity, [2] = Number of Orders (bug, zero)
        /// </summary>
        public string[][] asks { get; set; }
        public string[][] bids { get; set; }
    }
}