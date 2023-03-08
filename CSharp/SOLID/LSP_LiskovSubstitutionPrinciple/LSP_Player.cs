// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

namespace CSharp.SOLID.LSP_LiskovSubstitutionPrinciple
{
    /// <summary>
    /// According to the Liskov Substitution Principle, objects created from subclasses must exhibit the same behavior when
    /// they are replaced by objects of superclasses. Well; Derived classes must use all the properties of the derived classes.
    /// 
    /// In the code below, Striker actually includes the KeepTheBall method, which it does not need, so a striker cannot hold
    /// the ball with his hand. Under normal circumstances, this function will not be able to use, this function will need to
    /// throw an exception. In other words, there will be an unnecessary crowd of code and an additional effort in terms of
    /// code management, because there is actually an unnecessary method in the base class.
    /// 
    /// Ref : https://dijitalseruven.com/solid-nedir-solid-yazilim-prensipleri-nelerdir/#:~:text=SOLID%20prensipleri%20%3B%20geli%C5%9Ftirilen%20herhangi%20bir,ve%20yaz%C4%B1l%C4%B1m%20%C3%BCr%C3%BCn%C3%BCn%C3%BCn%20geli%C5%9Fmesini%20etkiler
    /// </summary>
    public abstract class LSP_Player
    {
        public virtual void KickTheBall()
        {
            // Ball was kicked !
        }
    }

    public interface IKeepTheBall
    {
        void KeepTheBall();
    }

    public class Striker : LSP_Player
    {
        public override void KickTheBall()
        {
            // Ball was kicked by Striker !
        }
    }

    public class Goalkeeper : LSP_Player, IKeepTheBall
    {
        public override void KickTheBall()
        {
            // Ball was kicked by Goalkeeper !
        }

        public void KeepTheBall()
        {
            // Ball was kept by Goalkeeper
        }
    }
}