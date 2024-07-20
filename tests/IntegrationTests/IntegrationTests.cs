using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Models.Public;

namespace integrationtests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async void V1ApiAcceptsV1PizzaOrder()
    {
        // Arrange
        var pizzaOrderNoPepperoni = new PizzaOrderV1(true, false, true);
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/v1/PizzaOrder", pizzaOrderNoPepperoni);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}