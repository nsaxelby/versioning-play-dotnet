using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Models.Public;
using Services;

namespace Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/PizzaOrder")]
    [ApiController]
    public class PizzaOrderControllerV1 : ControllerBase
    {
        private readonly IPizzaOrderService _pizzaOrderService;
        public PizzaOrderControllerV1(IPizzaOrderService pizzaOrderService)
        {
            _pizzaOrderService = pizzaOrderService;
        }

        [HttpPost]
        public IActionResult CreatePizzaOrder([FromBody] PizzaOrderV1 order)
        {
            string id = _pizzaOrderService.CreatePizzaOrder(
                new Models.Private.PizzaOrder(order.Cheese,
                 order.Pepperoni,
                  order.TomatoSauce));
            return StatusCode(201, id);
        }

        [HttpGet]
        public IActionResult GetPizzaOrder([FromQuery] string id)
        {
            var returnedValue = _pizzaOrderService.GetPizzaOrder(id);
            if (returnedValue == null)
            {
                return NotFound();
            }
            return Ok(new PizzaOrderV1(returnedValue.Cheese,
             returnedValue.Pepperoni,
              returnedValue.TomatoSauce));
        }
    }
}