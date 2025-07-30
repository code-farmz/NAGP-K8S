using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;
using WebApi.Contracts.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

       // api method to get all employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return Ok(employees);
        }

        // api method to create a new employee
        [HttpPost]
        public async Task<ActionResult> CreateEmployee([FromBody] EmployeeDto employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeService.CreateEmployeeAsync(employee);
            if (result)
            {
                return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
            }
            return StatusCode(500, "Internal server error");
        }
    }
}