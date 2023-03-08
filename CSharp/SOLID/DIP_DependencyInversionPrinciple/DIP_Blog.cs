// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

namespace CSharp.SOLID.DIP_DependencyInversionPrinciple
{
    /// <summary>
    /// - High-level classes should not depend on low-level classes, the relationship should be provided using abstraction
    /// or interface,
    /// - Abstraction should not depend on details, on the contrary, details should depend on abstractions.
    ///
    /// As a result of the operation, we have abstracted the lower level class Post, thanks to Interface, and reversed
    /// the dependency on the lower level class in our upper level class. In other words, the lower-level class Post has
    /// become dependent on Interface.
    /// 
    /// Ref : https://dijitalseruven.com/solid-nedir-solid-yazilim-prensipleri-nelerdir/#:~:text=SOLID%20prensipleri%20%3B%20geli%C5%9Ftirilen%20herhangi%20bir,ve%20yaz%C4%B1l%C4%B1m%20%C3%BCr%C3%BCn%C3%BCn%C3%BCn%20geli%C5%9Fmesini%20etkiler
    /// </summary>
    public class DIP_Blog
    {
        // High Level Class
        private IContent content;

        public DIP_Blog()
        {
            content = new DIP_Post();
        }

        public void Create()
        {
            content.CreatePost(true);
        }
    }

    public interface IContent
    {
        public void CreatePost(bool picture);
    }

    public class DIP_Post: IContent
    {
        // Low Level Method
        public void CreatePost(bool picture)
        {
            // Process
        }
    }
}