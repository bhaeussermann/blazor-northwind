using System.Text.Json;
using Northwind.Client.Models;

namespace Northwind.Client.Services;

internal sealed class EmployeeDataService
{
    private readonly HttpClient httpClient;

    public EmployeeDataService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(
            await this.httpClient.GetStreamAsync(""), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
