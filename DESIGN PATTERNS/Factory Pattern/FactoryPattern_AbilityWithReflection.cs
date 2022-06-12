// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ReflectionFactory
{
    /// <summary>
    /// - Factory Pattern -
    /// 
    /// Level 2
    /// 
    /// Ref : https://www.youtube.com/watch?v=FGVkio4bnPQ
    /// </summary>

    public abstract class FactoryPattern_AbilityWithReflection
    {
        public abstract string Name { get; }
        public abstract void Process();
    }
    
    public class StartFireAbility : FactoryPattern_AbilityWithReflection
    {
        
        public override string Name => "fire";
        // Also could be written as the following line
        //public override string Name { get { return "fire"; } }

        public override void Process()
        {
            // do some fire creation
        }
    }

    public class HealSelfAbility : FactoryPattern_AbilityWithReflection
    {
        public override string Name => "heal";
        public override void Process()
        {
            // self.Health++;
        }
    }
    
    public class AbilityFactory
    {
        public Dictionary<string, Type> abilitiesByName;

        // "Assembly.GetAssembly(typeof(FactoryPattern_AbilityWithReflection))"  -> finding the assembly that all of our
        // with ".GetTypes()" we're gonna get all of out classes when we call this.
        // Then we are filtering it with ".Where()", and we want to make sure that the type is a class because it could be 
        // a struct or sth else. We make sure that it is not abstract and that is a subclass of "FactoryPattern_AbilityWithReflection"
        
        // What is doing is giving all f the types in out project that are an ability but are not abstract
        public AbilityFactory()
        {
            var abilityTypes = Assembly.GetAssembly(typeof(FactoryPattern_AbilityWithReflection)).GetTypes()
                .Where(myType =>
                    myType.IsClass && !myType.IsAbstract &&
                    myType.IsSubclassOf(typeof(FactoryPattern_AbilityWithReflection)));
            
            // Dictionary for finding these by name later (could be an enum/id instead of string)
            abilitiesByName = new Dictionary<string, Type>();
            
            // Get the names and put them into the dictionary
            foreach (var type in abilityTypes)
            {
                // Creating types that we obtained from the previous steps.
                var tempEffect = Activator.CreateInstance(type) as FactoryPattern_AbilityWithReflection;
                abilitiesByName.Add(tempEffect.Name,type);
            }
        }
        
        public FactoryPattern_AbilityWithReflection GetAbility(string abilityType)
        {
            if (abilitiesByName.ContainsKey(abilityType))
            {
                Type type = abilitiesByName[abilityType];
                var ability = Activator.CreateInstance(type) as FactoryPattern_AbilityWithReflection;
                return ability;
            }

            return null;
        }

        internal IEnumerable<string> GetAbilityNames()
        {
            return abilitiesByName.Keys;
        }
    }
}

