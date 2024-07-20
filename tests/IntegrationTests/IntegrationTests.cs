using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Models.Public;
using Newtonsoft.Json;

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

    [Fact]
    public async void V1ApiReturnsV1PizzaOrder()
    {
        // Arrange
        var pizzaOrderNoPepperoni = new PizzaOrderV1(true, false, true);
        var client = _factory.CreateClient();

        // Act
        var response1 = await client.PostAsJsonAsync("api/v1/PizzaOrder", pizzaOrderNoPepperoni);
        string idOfPizza = await response1.Content.ReadAsStringAsync();
        var response2 = await client.GetAsync($"api/v1/PizzaOrder?id={idOfPizza}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var pizzaGot = JsonConvert.DeserializeObject<PizzaOrderV1>(await response2.Content.ReadAsStringAsync());
        Assert.Equal(pizzaOrderNoPepperoni, pizzaGot);
    }

    [Fact]
    public async void V2ApiAcceptsPizzaOrderWithCrust()
    {
        // Arrange
        var thinCrustpizzaOrderNoPepperoni = new PizzaOrderV2(true, false, true, "Thin");
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/v2/PizzaOrder", thinCrustpizzaOrderNoPepperoni);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async void V2ApiRejectsPizzaOrdersUnsupportedCrustSpecified()
    {
        // Arrange
        var pizzaOrderNoPepperoni = new PizzaOrderV2(true, false, true, "Garlic");
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/v2/PizzaOrder", pizzaOrderNoPepperoni);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Invalid Crust", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async void V2ApiRejectsPizzaOrdersWhenNoCrustSpecified()
    {
        // Arrange
        var pizzaOrderNoPepperoni = new PizzaOrderV1(true, false, true);
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/v2/PizzaOrder", pizzaOrderNoPepperoni);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("The Crust field is required", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async void V2ReturnsPizzaOrderWhenCreatedWithV1()
    {
        // Arrange
        var pizzaOrderNoPepperoni = new PizzaOrderV1(true, false, true);
        var client = _factory.CreateClient();

        // Act
        var response1 = await client.PostAsJsonAsync("api/v1/PizzaOrder", pizzaOrderNoPepperoni);
        string idOfPizza = await response1.Content.ReadAsStringAsync();
        var response2 = await client.GetAsync($"api/v2/PizzaOrder?id={idOfPizza}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var pizzaGot = JsonConvert.DeserializeObject<PizzaOrderV2>(await response2.Content.ReadAsStringAsync());
        Assert.NotNull(pizzaGot);
        Assert.Equal("Regular", pizzaGot.Crust);
        Assert.Equal(pizzaOrderNoPepperoni.Cheese, pizzaOrderNoPepperoni.Cheese);
        Assert.Equal(pizzaOrderNoPepperoni.Pepperoni, pizzaOrderNoPepperoni.Pepperoni);
        Assert.Equal(pizzaOrderNoPepperoni.TomatoSauce, pizzaOrderNoPepperoni.TomatoSauce);
    }

    [Fact]
    public async void V1ReturnsPizzaOrderWhenCreatedWithV2()
    {
        // Arrange
        var pizzaOrderNoPepperoni = new PizzaOrderV2(true, false, true, "Thin");
        var client = _factory.CreateClient();

        // Act
        var response1 = await client.PostAsJsonAsync("api/v2/PizzaOrder", pizzaOrderNoPepperoni);
        string idOfPizza = await response1.Content.ReadAsStringAsync();
        var response2 = await client.GetAsync($"api/v1/PizzaOrder?id={idOfPizza}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var pizzaGot = JsonConvert.DeserializeObject<PizzaOrderV1>(await response2.Content.ReadAsStringAsync());
        Assert.NotNull(pizzaGot);
        Assert.Equal(pizzaOrderNoPepperoni.Cheese, pizzaOrderNoPepperoni.Cheese);
        Assert.Equal(pizzaOrderNoPepperoni.Pepperoni, pizzaOrderNoPepperoni.Pepperoni);
        Assert.Equal(pizzaOrderNoPepperoni.TomatoSauce, pizzaOrderNoPepperoni.TomatoSauce);
    }
}