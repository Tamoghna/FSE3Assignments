using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductMicroService.Model;
using ProductMicroService.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Transactions;


namespace ProductMicroservice.Controllers
{
    [Route("e-auction/api/V1/Seller/[controller]")]
   
    [ApiController]
    public class DeleteController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public DeleteController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
            return new OkResult();
        }

        


    }
}