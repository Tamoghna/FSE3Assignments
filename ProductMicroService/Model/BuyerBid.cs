using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroService.Model
{
    public class BuyerBid
    {
        public int BuyerBidId { get; set; }
        public string BuyerFirstName { get; set; }
        public string BuyerLastName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerEmail { get; set; }
        public Int64 BuyerPhone { get; set; }
        public string BuyerCity { get; set; }
        public string BuyerState { get; set; }
        public int BuyerPin { get; set; }

        public int BidPrice { get; set; }
        public DateTime BidEndDate { get; set; }
        public int ProductId { get; set; }
    }
}
