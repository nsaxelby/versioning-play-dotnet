using Models.Public;

namespace Models.Private
{
    public class PizzaOrder
    {
        public string Id { get; init; }
        public bool Cheese { get; init; }
        public bool Pepperoni { get; init; }
        public bool TomatoSauce { get; init; }
        public Crust? Crust { get; init; }

        public PizzaOrder(bool cheese, bool pepperoni, bool tomatoSauce, Crust crust = Private.Crust.Regular)
        {
            Id = Guid.NewGuid().ToString();
            Cheese = cheese;
            Pepperoni = pepperoni;
            TomatoSauce = tomatoSauce;
            Crust = crust;
        }
    }

    public static class PizzaOrderExtensions
    {
        // This is where the version conversiton logic is, we prefer not to have two versions of data stored
        // We use one data storage PizzaOrder, and handle conversion here so that we can serve
        public static PizzaOrderV1 ToPublicV1(this PizzaOrder pizzaOrder)
        {
            return new PizzaOrderV1(pizzaOrder.Cheese, pizzaOrder.Pepperoni, pizzaOrder.TomatoSauce);
        }

        public static PizzaOrderV2 ToPublicV2(this PizzaOrder pizzaOrder)
        {
            // There is a choice here depending on the change that you make.
            // In this scenario, the pizza shop started with offering just Regular base, this means the data that we have (let us say in a database)
            // we can fill with a default value of 'Regular', or, alternatively, we can handle it in code (like we are doing here), and injecting a default that makes sense
            // We will wish to retire V1 eventually, which is why we chose to version this 'required' change behind a V2.
            // we could have done this behind a 'non breaking change' and just have crust as optional (defaulting to Regular), but the pizza shop owner prefers
            // to make it so the customer explicitly needs to set a crust type
            Crust crust = pizzaOrder.Crust ?? Crust.Regular;
            return new PizzaOrderV2(pizzaOrder.Cheese, pizzaOrder.Pepperoni, pizzaOrder.TomatoSauce, crust.ToString());
        }
    }
}