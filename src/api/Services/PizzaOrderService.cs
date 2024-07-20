using Models.Private;
namespace Services
{
    public interface IPizzaOrderService
    {
        string CreatePizzaOrder(PizzaOrder order);
        PizzaOrder? GetPizzaOrder(string id);
    }

    public class PizzaOrderService : IPizzaOrderService
    {
        private List<PizzaOrder> _orders = new List<PizzaOrder>();

        public string CreatePizzaOrder(PizzaOrder order)
        {
            _orders.Add(order);
            return order.Id;
        }

        public PizzaOrder? GetPizzaOrder(string id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }
    }
}