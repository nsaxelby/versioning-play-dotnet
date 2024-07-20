using System.ComponentModel;

namespace Models.Public
{
    // I have opted to not include the version name in the public schema name
    [DisplayName("PizzaOrder")]
    public class PizzaOrderV2
    {
        public bool Cheese { get; init; }
        public bool Pepperoni { get; init; }
        public bool TomatoSauce { get; init; }
        public string Crust { get; init; }

        public PizzaOrderV2(bool cheese, bool pepperoni, bool tomatoSauce, string crust)
        {
            Cheese = cheese;
            Pepperoni = pepperoni;
            TomatoSauce = tomatoSauce;
            Crust = crust;
        }

        public override bool Equals(object? obj)
        {
            if (obj is PizzaOrderV2 other)
            {
                return Cheese == other.Cheese && Pepperoni == other.Pepperoni && TomatoSauce == other.TomatoSauce && Crust == other.Crust;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Cheese, Pepperoni, TomatoSauce, Crust);
        }
    }
}