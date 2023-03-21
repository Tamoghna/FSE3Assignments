using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductMicroService;
using ProductMicroService.Model;
using ProductMicroService.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Transactions;


namespace ProductMicroservice.Controllers
{
    [Route("e-auction/api/V1/[controller]/place-bid")]
    [Route("e-auction/api/V1/[controller]/update-bid")]
    //[Route("e-auction/api/V1/[controller]/show-buyer")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        
        private readonly IProductRepository _productRepository;
        

        public BuyerController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        /*
        [HttpGet("{pageNumber}/{pageSize}")]
        public List<BuyerBid> GetBuyerBidsEntire()
        {

            // Return List of Customer  
            var source = _productRepository.GetBuyerBidsEntire();

            PagingParameterModel pagingparametermodel = new PagingParameterModel();
            // Get's No of Rows Count   
            int count = source.Count;

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            // Setting Header  
             HttpContext.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return items;

        }
        */
       

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }

        [HttpPost]
        public IActionResult Post([FromBody] BuyerBid bidproduct)
        {
            //Checks if the productid exists in the database
            var getproductid = _productRepository.GetProductByID(bidproduct.ProductId);

            if (getproductid == null)
            {
                return BadRequest();
            }

            DateTime bidEndDateForAProductId = _productRepository.GetProductByIDBidEndDate(bidproduct.ProductId);
            if (bidproduct.BidEndDate > bidEndDateForAProductId)
            {
                var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new System.Net.Http.StringContent("Bid is placed after End date."),
                    ReasonPhrase = "Bid is placed after End date."
                };

                throw new System.Web.Http.HttpResponseException(response);
                //return BadRequest();
            }
            int duplicateCount = _productRepository.GetBuyerBidByPoductIdEmail(bidproduct.ProductId, bidproduct.BuyerEmail);
            if (duplicateCount > 0)
            {
                var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new System.Net.Http.StringContent("More Than One Bid For a Product is Not Allowed"),
                    ReasonPhrase = "More Than One Bid For a Product is Not Allowed"
                };

                throw new System.Web.Http.HttpResponseException(response);
                //return BadRequest();
            }
            
            if (String.IsNullOrEmpty(bidproduct.BuyerFirstName))
            {
                return BadRequest();
            }
            if (bidproduct.BuyerFirstName.Length < 5)
            {
                return BadRequest();
            }
            if (bidproduct.BuyerFirstName.Length > 30)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(bidproduct.BuyerLastName))
            {
                return BadRequest();
            }
            if (bidproduct.BuyerLastName.Length < 3)
            {
                return BadRequest();
            }
            if (bidproduct.BuyerLastName.Length > 25)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(bidproduct.BuyerEmail))
            {
                return BadRequest();
            }

            if (!bidproduct.BuyerEmail.Contains("@"))
            {
                return BadRequest();
            }

            if (bidproduct.BuyerPhone.ToString().Length != 10)
            {
                return BadRequest();
            }
           

            using (var scope = new TransactionScope())
            {
                _productRepository.BidProduct(bidproduct);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = bidproduct.BuyerBidId }, bidproduct);
            }
        }

        [HttpPut]
        public IActionResult Put(int ProductId,string BuyerEmail,int BidPrice)
        {
            BuyerBid objUpdateBuyerBid = _productRepository.GetBuyerBidResultByProductIdEmail(ProductId, BuyerEmail);
            objUpdateBuyerBid.BidPrice = BidPrice;

            //Checks if the Current Date is greater than Bid End Date throw an exception
            if (DateTime.Now > objUpdateBuyerBid.BidEndDate)
            {
                //return BadRequest();
                string exceptionReason = "Current Date is greater than Bid End Date throw an exception";
                var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new System.Net.Http.StringContent(exceptionReason),
                    ReasonPhrase = exceptionReason
                };
                MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = method.Name;
                string className = method.ReflectedType.Name;
                string fullMethodName = className + "." + methodName;
                

                Logger objLogger = new Logger();
                objLogger.ExceptionFileLogging(exceptionReason, fullMethodName);

                throw new System.Web.Http.HttpResponseException(response);  
            }

            

            using (var scope = new TransactionScope())
                {
                    _productRepository.UpdateBidAmountByBuyer(objUpdateBuyerBid);
                    scope.Complete();
                    return new OkResult();
                }
           
            return new NoContentResult();
        }

    }
}