using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Northwind.Client.Models;
using Northwind.Client.Services;

namespace Northwind.Client.Pages;

public partial class EditEmployee
{
    [Parameter]
    public string EmployeeId { get; set; }

    public Employee Employee { get; private set; } = new();

    public bool IsEditing => EmployeeId != null;
    public bool IsLoading { get; private set; }
    public bool DidLoadingFail { get; private set; }
    public bool IsSaving { get; private set; }

    [Inject]
    private EmployeeDataService EmployeeDataService { get; set; }

    [Inject]
    private IJSRuntime JavaScriptRuntime { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private NotificationService NotificationService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (EmployeeId != null)
        {
            IsLoading = true;
            try
            {
                Employee = await EmployeeDataService.Get(int.Parse(EmployeeId));
            }
            catch (ApiException exception)
            {
                DidLoadingFail = true;
                NotificationService.NotifyError("Error loading employee", exception.Message);
            }
            finally
            {
                IsLoading = false;
            }

            if (!DidLoadingFail)
            {
                await FocusFirstField(waitForNextEventLoop: true);
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && !IsEditing)
        {
            await FocusFirstField();
        }
    }

    private async Task FocusFirstField(bool waitForNextEventLoop = false)
    {
        await JavaScriptRuntime.InvokeVoidAsync("eval", waitForNextEventLoop
            ? "setTimeout(() => document.getElementById('last-name').focus())"
            : "document.getElementById('last-name').focus()");
    }

    protected async Task HandleValidSubmit()
    {
        IsSaving = true;
        try
        {
            if (IsEditing)
            {
                await EmployeeDataService.Update(Employee);
            }
            else
            {
                await EmployeeDataService.Add(Employee);
            }
            NavigationManager.NavigateTo("/employees");
        }
        catch (ApiException exception)
        {
            NotificationService.NotifyError($"Error {(IsEditing ? "updating" : "adding")} employee", exception.Message);
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
