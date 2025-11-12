using AutoMapper;
using Firmeza.Application.DTOs.Customers;
using Firmeza.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        // ✅ GET api/customers
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerIndexViewModel>>> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // ✅ GET api/customers/{id}
        [Authorize(Roles = "Administrator,Customer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerEditViewModel>> GetById(int id)
        {
            var customer = await _customerService.GetCustomerForEditAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // ✅ POST api/customers
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<CustomerIndexViewModel>> Create(CustomerCreateViewModel model)
        {
            try
            {
                var created = await _customerService.CreateCustomerAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ PUT api/customers/{id}
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CustomerEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            try
            {
                var success = await _customerService.UpdateCustomerAsync(model);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ DELETE api/customers/{id}
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _customerService.DeleteCustomerAsync(id);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
