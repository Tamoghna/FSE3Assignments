using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroService.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductShortDescription { get; set; }
        public string ProductDetailedDescription { get; set; }

        public int BidStartingPrice { get; set; }
        public DateTime BidEndDate { get; set; }
        public int CategoryId { get; set; }
        public string SellerFirstName { get; set; }
        public string SellerLastName { get; set; }
        public string SellerAddress { get; set; }
        public string SellerEmail { get; set; }
        public Int64 SellerPhone { get; set; }
        public string SellerCity { get; set; }
        public string SellerState { get; set; }
        public int SellerPin { get; set; }
    }
}
