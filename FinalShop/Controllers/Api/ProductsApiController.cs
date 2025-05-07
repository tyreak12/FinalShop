using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalShop.Models;
using FinalShop.Services;

namespace FinalShop.Controllers.Api
{
    [ApiController]
    [Route("api/ProductsApi")]
    [Authorize] 
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsApiController(IProductService service)
            => _service = service;

        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
            => Ok(_service.GetAll());

        // GET: api/products/5
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var prod = _service.GetById(id);
            if (prod == null) return NotFound();
            return Ok(prod);
        }

        // POST: api/products
        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _service.Create(product);
            return CreatedAtAction(nameof(Get), new { id = product.productID }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            if (id != product.productID) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _service.Update(product);
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}
