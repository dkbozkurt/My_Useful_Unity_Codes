// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;

namespace CSharp8
{
    public enum Direction
    {
        Up,Down,Right,Left
    }

    public enum Orientation
    {
        North,South,East,West
    }
    
    public enum PaymentMethods
    {
        CreditCard,
        WireTransfer
    }
    
    /// <summary>
    /// Pattern Matching : Pattern matching is the process of taking an expression
    /// and testing whether it matches certain criteria, such as “being a specified
    /// type” or “matching a specified constant value”.
    /// 
    /// Ref : https://dotnettutorials.net/lesson/pattern-matching-in-csharp-8/
    /// </summary>
    public class PatternMatchingEnhancements
    {
        public static void SwitchExpressions()
        {
            Console.WriteLine("Enter the Day");
            string day = Console.ReadLine();
            var message = day.ToUpper() switch
            {
                "SUNDAY" => "Weekend",
                "MONDAY" => "Start of the weekday",
                "FRIDAY" => "End of the weekday",
                _ => "Invalid day"
            };
            
            Console.WriteLine($"{day} is {message}");
        }

        public static void SwitchExpression_WithEnums()
        {
            var direction = Direction.Left;
            Console.WriteLine($"Map view direction is {direction}");

            Orientation? orientation = direction switch
            {
                Direction.Up => Orientation.North,
                Direction.Down => Orientation.South,
                Direction.Left => Orientation.West,
                Direction.Right => Orientation.East,
                _=> throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
            };
            
            Console.WriteLine($"Cardinal Orientation is {orientation}");
        }

        public static void PropertyPattern_CalculateSalesTax()
        {
            double salePrice = 100;
            StateAddress address = new StateAddress {State = "CALIFORNIA"};
            var salesTax = ComputeSalesTax(address, salePrice);
            Console.WriteLine($"State: {address.State}, salePrice: {salePrice}, and salesTax: {salesTax}");
        }

        private static double ComputeSalesTax(StateAddress location, double salePrice)
        {
            var salesTax = location switch
            {
                { State: "NEWYORK" } => salePrice * 0.06,
                { State: "CALIFORNIA" } => salePrice * 0.075,
                { State: "TEXAS" } => salePrice * 0.05,
                _ => 0
            };
            return salesTax;
        }

        public static void TuplePattern_OrderDiscount()
        {
            CustomerOrder customerOrder1 = new CustomerOrder()
            {
                PaymentMethod = PaymentMethods.CreditCard,
                Country = "Turkey",
                Amount = 2000
            };
            CustomerOrder customerOrder2 = new CustomerOrder()
            {
                PaymentMethod = PaymentMethods.WireTransfer,
                Country = "USA",
                Amount = 3000
            };

            Console.WriteLine($"Country: {customerOrder2.Country}, Payment Method : {customerOrder1.PaymentMethod}, Order Discount : {GetOrderDiscount(customerOrder1)}");
            Console.WriteLine($"Country: {customerOrder2.Country}, Payment Method : {customerOrder2.PaymentMethod}, Order Discount : {GetOrderDiscount(customerOrder2)}");
        }
        
        public static int GetOrderDiscount(CustomerOrder order)
        {
            return (order.PaymentMethod, order.Country) switch
            {
                (PaymentMethods.CreditCard, "Turkey") => 20,
                (PaymentMethods.WireTransfer, "USA") => 15,
                (_, _) when order.Amount > 5000 => 10,
                _ => 0
            };
        }

        public static void PositionalPattern_Shipping()
        {
            Customer customer = new Customer()
            {
                FirstName = "DogukanKaan",
                LastName = "Bozkurt",
                Email = "dkaanbozkurt@gmail.com",
                CustomerAddress = new Address() { PostalCode = 755019, Street = "Ankara", Country = "Turkey"}
            };

            Console.WriteLine($"Is Free Shipping Eligible : {IsFreeShippingEligible(customer)}");
        }
        
        public static bool IsFreeShippingEligible(Customer customer)
        { 
            return customer is Customer(_, _, _, (_, _, "USA"));
        }

    }

    public class StateAddress
    {
        public string State { get; set; }
    }
    
    public class CustomerOrder
    {
        public PaymentMethods PaymentMethod { get; set; }
        public string Country { get; set; }
        public double Amount { get; set; }
    }
    
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Address CustomerAddress { get; set; }

        public void Deconstruct(out string firstname, out string lastname, out string email, out Address address)
        {
            firstname = FirstName;
            lastname = LastName;
            email = Email;
            address = CustomerAddress;
        }
    }

    public class Address
    {
        public int PostalCode { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public void Deconstruct(out int postalCode, out string street, out string country)
        {
            postalCode = PostalCode;
            street = Street;
            country = Country;
        }
    }
    
}