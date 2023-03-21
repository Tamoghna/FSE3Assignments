using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ProductBidCloud.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
