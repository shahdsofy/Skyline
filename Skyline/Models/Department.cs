using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Skyline.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ValidateNever]
        public List<Employee>Employees { get; set; }
    }
}
