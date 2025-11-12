using Firmeza.Application.DTOs.Sales;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController:ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _saleService.GetAllSalesAsync();
        return Ok(sales);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sale = await _saleService.GetSaleForEditAsync(id);
        if (sale == null)
        {
            return NotFound(new { message = "Sale not found" });
        }

        return Ok(sale);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaleCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _saleService.CreateSaleAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception e)
        {
           return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SaleEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest(new { message = "Mismatched Sale ID" });
        }

        try
        {
            var update = await _saleService.UpdateSaleAsync(model);
            if (!update)
                return NotFound(new { message = "Sale not found" });
            
            return Ok(new { message = "Sale updated" });
        }
        catch (Exception e)
        {   
           return BadRequest(new { message = e.Message });
        }
        
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _saleService.DeleteSaleAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Sale not found" });
            }
            
            return Ok(new { message = "Sale deleted successfully" });
        }
        catch (Exception e)
        {
           return BadRequest(new { message = e.Message });
        }
    }
    
}