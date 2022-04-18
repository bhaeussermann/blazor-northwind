using Microsoft.AspNetCore.Components;
using Northwind.Client.Models;
using Northwind.Client.Services;

namespace Northwind.Client.Pages;

public partial class Employees
{
    public IEnumerable<Employee> EmployeesList { get; private set; } = Array.Empty<Employee>();

    [Inject]
    private EmployeeDataService EmployeeDataService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        EmployeesList = (await EmployeeDataService.GetAll()).ToArray();
    }
}
