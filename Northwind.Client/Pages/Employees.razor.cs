using Microsoft.AspNetCore.Components;
using Northwind.Client.Models;
using Northwind.Client.Services;

namespace Northwind.Client.Pages;

public partial class Employees
{
    private IEnumerable<Employee> employees;

    public bool IsLoading => (this.employees == null) && !DidFailLoading;

    public bool DidLoad => this.employees != null;

    public bool DidFailLoading { get; private set; }

    public IEnumerable<Employee> FilteredEmployeesList
    {
        get
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return this.employees;
            }

            return this.employees.Where(e =>
                e.LastName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)
                || e.FirstName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)
                || e.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public string SearchText { get; set; }

    [Inject]
    private EmployeeDataService EmployeeDataService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private NotificationService NotificationService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        DidFailLoading = false;
        try
        {
            this.employees = (await EmployeeDataService.GetAll())
                .OrderBy(e => e.LastName)
                .ToArray();
        }
        catch (ApiException exception)
        {
            NotificationService.NotifyError("Error loading employees", exception.Message);
            DidFailLoading = true;
        }
    }

    protected void AddEmployee()
    {
        NavigationManager.NavigateTo("/employees/add");
    }
}
