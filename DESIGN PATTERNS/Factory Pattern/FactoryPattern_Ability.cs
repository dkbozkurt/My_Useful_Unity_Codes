// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

namespace SimpleFactory
{
    
    /// <summary>
    /// - Factory Pattern -
    ///
    /// Level 1
    /// Very Simple Example of Factory Pattern.
    /// Here the core piece of functionally is AbilityFactory class.
    ///  
    /// Ref : https://www.youtube.com/watch?v=FGVkio4bnPQ
    /// </summary>
    
    public abstract class FactoryPattern_Ability
    {
        public abstract void Process();
    }

    public class StartFireAbility : FactoryPattern_Ability
    {
        public override void Process()
        {
            // do some fire creation
        }
    }

    public class HealSelfAbility : FactoryPattern_Ability
    {
        public override void Process()
        {
            // self.Health++;
        }
    }
    
    // This is our factory. Factory responsible for returning back either a start fire ability or heal self ability
    // depending on what we want.
    public class AbilityFactory
    {
        public FactoryPattern_Ability GetAbility(string abilityType)
        {
            switch (abilityType)
            {
                case "fire":
                    return new StartFireAbility();
                case "heal":
                    return new HealSelfAbility();
                default:
                    return null;
            }
        }
    }
}

