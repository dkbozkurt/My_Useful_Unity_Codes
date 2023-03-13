// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;

namespace CSharp8
{
    /// <summary>
    /// When you create a readonly structure,
    /// it is necessary to use a readonly modifier
    /// with its fields.
    /// 
    /// Ref : https://www.geeksforgeeks.org/readonly-in-c-sharp/
    /// </summary>
    public struct Product
    {
        public readonly string Name { get; }
        public readonly int Label { get; }
        public int Amount { get; set; }

        public Product(string name,int label,int amount)
        {
            this.Name = name;
            this.Label = label;
            this.Amount = amount;
        }
    }
    
    /// <summary>
    /// Readonly members can only be assigned or
    /// reassigned multiple times only at the
    /// declaration or in a constructor.
    ///
    /// Ref : https://www.geeksforgeeks.org/readonly-in-c-sharp/
    /// </summary>
    public class ReadonlyMembers
    {
        public readonly string name;
        public readonly string surname;
        public readonly int age;

        public readonly string Topic = "readonlyMembers";

        public ReadonlyMembers(string a, string b, int c)
        {
            name = a;
            surname = b;
            age = c;
        }
        
        public static void StartReadonlyClass()
        {
            ReadonlyMembers readOnlyMember = new ReadonlyMembers("DogukanKaan", "Bozkurt",24);

            Console.WriteLine("Name: {0} \nSurname: {1}\nAge: {2}\nTopic :{3}",
                readOnlyMember.name,readOnlyMember.surname,readOnlyMember.age,readOnlyMember.Topic);
            
            // Below line will give error because it is trying to change readonly field.
            // readOnlyMember.name = "Kaan";
        }

        public static void StartReadonlyStruct()
        {
            Product appleProduct = new Product("Apple", 321321, 1000);
            Console.WriteLine("Product Name: {0} \nProduct Label: {1}\nAmount: {2}",
                appleProduct.Name,appleProduct.Label,appleProduct.Amount);

            // Not editable
            // appleProduct.Label = "55555";

            appleProduct.Amount = 500;
            
            Console.WriteLine("After Amount decreased\nNew product amount: " + appleProduct.Amount);
        }
    }
}