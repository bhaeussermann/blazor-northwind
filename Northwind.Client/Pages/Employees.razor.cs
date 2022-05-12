using Microsoft.AspNetCore.Components;
using Northwind.Client.Models;
using Northwind.Client.Services;

namespace Northwind.Client.Pages;

public partial class Employees
{
    private IEnumerable<Employee> employees;

    public bool IsLoading { get; private set; }

    public bool DidLoad => this.employees != null;

    public bool DidFailLoading { get; private set; }

    public Employee EmployeeBeingDeleted { get; private set; }

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

    [Inject]
    private Radzen.DialogService DialogService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadEmployees();
    }

    protected void AddEmployee()
    {
        NavigationManager.NavigateTo("/employees/add");
    }

    protected void EditEmployee(Employee employee)
    {
        NavigationManager.NavigateTo("/employees/" + employee.Id);
    }

    protected async Task DeleteEmployee(Employee employee)
    {
        bool? dialogResult = await DialogService.Confirm(
            $"Delete employee {employee.FirstName} {employee.LastName}?",
            "Delete employee",
            new Radzen.ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        if (dialogResult == true)
        {
            EmployeeBeingDeleted = employee;
            StateHasChanged();
            try
            {
                await EmployeeDataService.Delete(employee);
            }
            catch (ApiException exception)
            {
                NotificationService.NotifyError("Error deleting employee", exception.Message);
                return;
            }
            finally
            {
                EmployeeBeingDeleted = null;
            }

            await LoadEmployees();
        }
    }

    private async Task LoadEmployees()
    {
        IsLoading = true;
        DidFailLoading = false;
        StateHasChanged();

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
        finally
        {
            IsLoading = false;
        }
    }
}
