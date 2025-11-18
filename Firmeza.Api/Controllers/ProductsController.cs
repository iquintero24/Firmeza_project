using AutoMapper;
using Firmeza.Application.DTOs.Products;
using Firmeza.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController: ControllerBase
{
   private readonly IProductService _productService;
   private readonly IMapper _mapper;

   public ProductsController(IProductService productService, IMapper mapper)
   {
      _productService = productService;
      _mapper = mapper;
   }
   
   // Get api/products
   [HttpGet]
   public async Task<ActionResult<IEnumerable<ProductIndexViewModel>>> GetAll()
   {
      var products = await _productService.GetAllProductsAsync();
      return Ok(products);
   }
   
   // Get api/products/{id}
   [HttpGet("{id}")]
   public async Task<ActionResult<ProductEditViewModel>> GetById(int id)
   {
      var product = await _productService.GetProductForEditAsync(id);
      if (product == null) return NotFound();
      return Ok(product);
   }
   
   // POST api/products
   [HttpPost]
   public async Task<ActionResult<ProductIndexViewModel>> Create(ProductCreateViewModel model)
   {
      var created = await _productService.CreateProductAsync(model);
      return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
   }

   // PUT api/products/{id}
   [HttpPut("{id}")]
   public async Task<IActionResult> Update(int id, ProductEditViewModel model)
   {
      if (id != model.Id)
         return BadRequest();

      var success = await _productService.UpdateProductAsync(model);
      if (!success)
         return NotFound();

      return NoContent();
   }

   // DELETE api/products/{id}
   [HttpDelete("{id}")]
   public async Task<IActionResult> Delete(int id)
   {
      var success = await _productService.DeleteProductAsync(id);
      if (!success)
         return NotFound();

      return NoContent();
   }
}
