using WebApi.Contracts.DTO;

namespace WebApi.Contracts.Interface
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync();
        Task<bool> CreateEmployeeAsync(EmployeeDto employee);
    }
}