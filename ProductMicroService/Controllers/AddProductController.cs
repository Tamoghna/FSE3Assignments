using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductMicroService;
using ProductMicroService.Model;
using ProductMicroService.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Transactions;


namespace ProductMicroservice.Controllers
{
    [Route("e-auction/api/V1/Seller/[controller]")]
    [Route("e-auction/api/V1/Seller/show-bids")]
    [ApiController]
    [EnableCors("Access-Control-Allow-Origin")]
    public class AddProductController : ControllerBase
    {
        
        private readonly IProductRepository _productRepository;

        public AddProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }
        
        [HttpGet("{productid}")]
        public List<ShowBuyerBidDetails> GetProductById(int productid, [FromQuery] int PageNumber,
                                             [FromQuery] int PageSize)
        {
            return _productRepository.ShowProductBidsByProductId(productid, PageNumber, PageSize);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            #region ValidationLogic
            if (product.CategoryId > 3)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(product.ProductName))
            {
                return BadRequest();
            }
            if (product.ProductName.Length < 5)
            {
                return BadRequest();
            }
            if (product.ProductName.Length >30 )
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(product.SellerFirstName))
            {
                return BadRequest();
            }
            if (product.SellerFirstName.Length < 5)
            {
                return BadRequest();
            }
            if (product.SellerFirstName.Length > 30)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(product.SellerLastName))
            {
                return BadRequest();
            }
            if (product.SellerLastName.Length < 3)
            {
                return BadRequest();
            }
            if (product.SellerLastName.Length > 25)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(product.SellerEmail))
            {
                return BadRequest();
            }

            if (!product.SellerEmail.Contains("@"))
            {
                return BadRequest();
            }

            if (product.SellerPhone.ToString().Length != 10)
            {
                return BadRequest();
            }

            if (product.BidEndDate < DateTime.Today)
            {
                string exceptionReason = "The Bid End Date Should be future date";
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
            #endregion

            using (var scope = new TransactionScope())
            {
                _productRepository.InsertProduct(product);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
            return new OkResult();
        }


    }
}