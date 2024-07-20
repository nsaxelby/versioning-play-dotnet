namespace Models.Private
{
    public class PizzaOrder
    {
        public string Id { get; init; }
        public bool Cheese { get; init; }
        public bool Pepperoni { get; init; }
        public bool TomatoSauce { get; init; }

        public PizzaOrder(bool cheese, bool pepperoni, bool tomatoSauce)
        {
            Id = Guid.NewGuid().ToString();
            Cheese = cheese;
            Pepperoni = pepperoni;
            TomatoSauce = tomatoSauce;
        }
    }
}