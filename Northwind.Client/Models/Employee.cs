using System.Text.Json.Serialization;

namespace Northwind.Client.Models;

public class Employee
{
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate { get; set; }
}
