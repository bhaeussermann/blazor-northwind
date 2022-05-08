using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Northwind.Client.Models;
using Northwind.Client.Services;

namespace Northwind.Client.Pages;

public partial class EditEmployee
{
    public Employee Employee { get; } = new();
    public bool IsSaving { get; private set; }

    [Inject]
    private EmployeeDataService EmployeeDataService { get; set; }

    [Inject]
    private IJSRuntime JavaScriptRuntime { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private NotificationService NotificationService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            await JavaScriptRuntime.InvokeVoidAsync("eval", "document.getElementById('last-name').focus()");
        }
    }

    protected async Task HandleValidSubmit()
    {
        IsSaving = true;
        try
        {
            await EmployeeDataService.Add(Employee);
            NavigationManager.NavigateTo("/employees");
        }
        catch (ApiException exception)
        {
            NotificationService.NotifyError("Error adding employee", exception.Message);
        }
        finally
        {
            IsSaving = false;
        }
    }

    protected void CancelEdit()
    {
        NavigationManager.NavigateTo("/employees");
    }
}
