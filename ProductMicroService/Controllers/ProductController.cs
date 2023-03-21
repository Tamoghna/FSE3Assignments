using Microsoft.AspNetCore.Mvc;
using ProductMicroService.Model;
using ProductMicroService.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Transactions;


namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
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
                throw new SystemException("The Bid End Date Should be future date");
            }
            using (var scope = new TransactionScope())
            {
                _productRepository.InsertProduct(product);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
        }
    }
}