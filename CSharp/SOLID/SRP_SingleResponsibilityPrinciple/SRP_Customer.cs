// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

namespace CSharp.SOLID.SRP_SingleResponsibilityPrinciple
{
    /// <summary>
    /// According to the Single Responsibility Principle, each method and class has a single task and responsibility.
    ///
    /// According to SRP, a class should take only one responsibility. That's why we create a class named File and
    /// transfer the SaveToFile function there. Thus, we are separating two separate tasks from each other.
    /// 
    /// Ref : https://dijitalseruven.com/solid-nedir-solid-yazilim-prensipleri-nelerdir/#:~:text=SOLID%20prensipleri%20%3B%20geli%C5%9Ftirilen%20herhangi%20bir,ve%20yaz%C4%B1l%C4%B1m%20%C3%BCr%C3%BCn%C3%BCn%C3%BCn%20geli%C5%9Fmesini%20etkiler
    /// </summary>
    public class SRP_Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public void CreateCustomer(SRP_Customer srpCustomer)
        {
            // Customer created !
        }

        // Should be moved to another class ! As it is moved below.
        /*
        public void SaveToFile(SRP_Customer srpCustomer)
        {
            // Customer Saved to file !
        }
        */
    }

    
    public class SRP_File
    {
        public void SaveToFile(SRP_Customer srpCustomer)
        {
            // Customer Saved to file !
        }
    }
}
