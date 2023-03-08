// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

namespace CSharp.SOLID.ISP_InterfaceSegregationPrinciple
{
    /// <summary>
    /// According to the Interface Segregation principle, each interface must have a specific purpose. Instead of using
    /// a single interface that covers all methods, several interfaces are preferred, each serving separate method groups.
    /// 
    /// Ref : https://dijitalseruven.com/solid-nedir-solid-yazilim-prensipleri-nelerdir/#:~:text=SOLID%20prensipleri%20%3B%20geli%C5%9Ftirilen%20herhangi%20bir,ve%20yaz%C4%B1l%C4%B1m%20%C3%BCr%C3%BCn%C3%BCn%C3%BCn%20geli%C5%9Fmesini%20etkiler
    /// </summary>
    public interface ISP_IPost
    {
        public void CreatePost();
        public void ReadPost();
    }

    public interface IPostCreate
    {
        public void CreatePost();
    }

    public interface IPostRead
    {
        public void ReadPost();
    }
}