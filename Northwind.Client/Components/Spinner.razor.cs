using Microsoft.AspNetCore.Components;

namespace Northwind.Client.Components;

public partial class Spinner
{
    [Parameter]
    public bool IsInline { get; set; }
}
