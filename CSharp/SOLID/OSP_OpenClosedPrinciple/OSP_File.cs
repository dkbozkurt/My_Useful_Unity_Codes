// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using CSharp.SOLID.SRP_SingleResponsibilityPrinciple;

namespace CSharp.SOLID.OSP_OpenClosedPrinciple
{
    /// <summary>
    /// According to the Open Closed Principle, every class should be open to development but not closed to change.
    ///
    /// So, if we want to save to a new file type, we just apply the Inheritance operation from the File class structure.
    /// As a result, the File class structure is open for development but closed for modification.
    /// 
    /// Ref : https://dijitalseruven.com/solid-nedir-solid-yazilim-prensipleri-nelerdir/#:~:text=SOLID%20prensipleri%20%3B%20geli%C5%9Ftirilen%20herhangi%20bir,ve%20yaz%C4%B1l%C4%B1m%20%C3%BCr%C3%BCn%C3%BCn%C3%BCn%20geli%C5%9Fmesini%20etkiler
    /// </summary>
    public abstract class OSP_File
    {
        public abstract void SaveToFile(SRP_Customer customer);
    }

    public class TxtFile : OSP_File
    {
        public override void SaveToFile(SRP_Customer customer)
        {
            // Saved to txt file!
        }
    }
    
    public class XlsFile : OSP_File
    {
        public override void SaveToFile(SRP_Customer customer)
        {
            // SAve to xls file !
        }
    }
}