using AutoMapper;
using Firmeza.Application.DTOs.Customers;
using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController: ControllerBase
{
    private readonly ICustomerRepository _customerService;
    private readonly IMapper _mapper;

    public CustomersController(IMapper mapper, ICustomerRepository customerService)
    {
        _customerService = customerService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerIndexViewModel>>> GetCustomers()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }
}