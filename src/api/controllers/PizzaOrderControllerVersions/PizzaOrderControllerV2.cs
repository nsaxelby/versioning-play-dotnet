using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Models.Private;
using Models.Public;
using Services;

namespace Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/PizzaOrder")]
    [ApiController]
    public class PizzaOrderControllerV2 : ControllerBase
    {
        private readonly IPizzaOrderService _pizzaOrderService;
        public PizzaOrderControllerV2(IPizzaOrderService pizzaOrderService)
        {
            _pizzaOrderService = pizzaOrderService;
        }

        [HttpPost]
        public IActionResult CreatePizzaOrder([FromBody] PizzaOrderV2 order)
        {
            if (Enum.TryParse(order.Crust, out Crust crustSelected) == false)
            {
                return BadRequest("Invalid Crust, must be one of: " + EnumHelper.ToStringList(typeof(Crust)));
            }

            string id = _pizzaOrderService.CreatePizzaOrder(
                new PizzaOrder(order.Cheese,
                 order.Pepperoni,
                  order.TomatoSauce,
                  crustSelected));

            return StatusCode((int)HttpStatusCode.Created, id);
        }

        [HttpGet]
        public IActionResult GetPizzaOrder([FromQuery] string id)
        {
            var returnedValue = _pizzaOrderService.GetPizzaOrder(id);
            if (returnedValue == null)
            {
                return NotFound();
            }
            return Ok(returnedValue.ToPublicV2());
        }
    }
}