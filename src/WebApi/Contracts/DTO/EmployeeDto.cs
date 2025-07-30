using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.DTO;

public class EmployeeDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}