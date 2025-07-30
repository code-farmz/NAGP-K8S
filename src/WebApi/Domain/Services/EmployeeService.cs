using Microsoft.EntityFrameworkCore;
using WebApi.Data.Models;
using WebApi.Contracts.DTO;
using WebApi.Contracts.Interface;

namespace WebApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ApplicationDbContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        //create implementation for GetEmployeesAsync
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();
                return employees.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employees");
                throw;
            }
        }
        //create implementation for CreateEmployeeAsync
        public async Task<bool> CreateEmployeeAsync(EmployeeDto employee)
        {
            try
            {
                var newEmployee = new Employee
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return false;
            }
        }
    }
}
