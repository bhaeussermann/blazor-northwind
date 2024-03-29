﻿using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        var response = await this.httpClient.GetAsync("");
        await HandleErrorResponse(response);
        return JsonSerializer.Deserialize<IEnumerable<Employee>>(await response.Content.ReadAsStringAsync());
    }

    public async Task<Employee> Get(int employeeId)
    {
        var response = await this.httpClient.GetAsync(employeeId.ToString());
        await HandleErrorResponse(response);
        return JsonSerializer.Deserialize<Employee>(await response.Content.ReadAsStringAsync());
    }

    public async Task Add(Employee employeeToAdd)
    {
        var response = await this.httpClient.PostAsync("", new StringContent(JsonSerializer.Serialize(employeeToAdd)));
        await HandleErrorResponse(response);
    }

    public async Task Update(Employee employeeToUpdate)
    {
        var response = await this.httpClient.PutAsync(employeeToUpdate.Id.ToString(), new StringContent(JsonSerializer.Serialize(employeeToUpdate)));
        await HandleErrorResponse(response);
    }

    public async Task Delete(Employee employeeToDelete)
    {
        var response = await this.httpClient.DeleteAsync(employeeToDelete.Id.ToString());
        await HandleErrorResponse(response);
    }

    private static async Task HandleErrorResponse(HttpResponseMessage response)
    {
        if (response.StatusCode != HttpStatusCode.OK)
        {
            string errorResponseString = await response.Content.ReadAsStringAsync();
            if (errorResponseString[0] == '{')
            {
                var responseBody = JsonSerializer.Deserialize<ApiError>(errorResponseString);
                throw new ApiException(responseBody.Message);
            }
            throw new ApiException(errorResponseString);
        }
    }

    private class ApiError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
