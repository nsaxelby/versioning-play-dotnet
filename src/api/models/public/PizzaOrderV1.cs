using System.ComponentModel;

namespace Models.Public
{
    // I have opted to not include the version name in the public schema name
    [DisplayName("PizzaOrder")]
    public class PizzaOrderV1
    {
        public bool Cheese { get; init; }
        public bool Pepperoni { get; init; }
        public bool TomatoSauce { get; init; }

        public PizzaOrderV1(bool cheese, bool pepperoni, bool tomatoSauce)
        {
            Cheese = cheese;
            Pepperoni = pepperoni;
            TomatoSauce = tomatoSauce;
        }
    }
}