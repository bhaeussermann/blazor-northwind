using Northwind.Models;

namespace Northwind.Pages;

public partial class Employees
{
    public IEnumerable<Employee> EmployeesList { get; private set; }

    public Employees()
    {
        EmployeesList = new List<Employee>
        {
            new Employee { FirstName = "Tim", LastName = "Berners-Lee" },
            new Employee { FirstName = "Steve", LastName = "Buchanan" },
            new Employee { FirstName = "Laura", LastName = "Callahan" }
        };
    }
}
