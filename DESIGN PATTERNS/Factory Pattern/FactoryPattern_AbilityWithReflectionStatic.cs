// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ReflectionFactoryStatic
{
    /// <summary>
    /// - Factory Pattern -
    /// 
    /// Level 3
    /// 
    /// Ref : https://www.youtube.com/watch?v=FGVkio4bnPQ
    /// </summary>
    
    public abstract class FactoryPattern_AbilityWithReflectionStatic
    {
        public abstract string Name { get; }
        public abstract void Process();
    }
    
    public class StartFireAbility : FactoryPattern_AbilityWithReflectionStatic
    {
        
        public override string Name => "fire";
        // Also could be written as the following line
        //public override string Name { get { return "fire"; } }

        public override void Process()
        {
            // do some fire Ga
        }
    }

    public class HealSelfAbility : FactoryPattern_AbilityWithReflectionStatic
    {
        public override string Name => "heal";
        public override void Process()
        {
            // self.Health++;
        }
    }
    
    public static class AbilityFactory
    {
        private static Dictionary<string, Type> abilitiesByName;
        private static bool IsInıtialized => abilitiesByName != null;

        // "Assembly.GetAssembly(typeof(FactoryPattern_AbilityWithReflection))"  -> finding the assembly that all of our
        // with ".GetTypes()" we're gonna get all of out classes when we call this.
        // Then we are filtering it with ".Where()", and we want to make sure that the type is a class because it could be 
        // a struct or sth else. We make sure that it is not abstract and that is a subclass of "FactoryPattern_AbilityWithReflection"
        
        // What is doing is giving all f the types in out project that are an ability but are not abstract
        public static void InitializeFactory()
        {
            if(IsInıtialized) return;
            
            var abilityTypes = Assembly.GetAssembly(typeof(FactoryPattern_AbilityWithReflectionStatic)).GetTypes()
                .Where(myType =>
                    myType.IsClass && !myType.IsAbstract &&
                    myType.IsSubclassOf(typeof(FactoryPattern_AbilityWithReflectionStatic)));
            
            // Dictionary for finding these by name later (could be an enum/id instead of string)
            abilitiesByName = new Dictionary<string, Type>();
            
            // Get the names and put them into the dictionary
            foreach (var type in abilityTypes)
            {
                // Creating types that we obtained from the previous steps.
                var tempEffect = Activator.CreateInstance(type) as FactoryPattern_AbilityWithReflectionStatic;
                abilitiesByName.Add(tempEffect.Name,type);
            }
        }
        
        public static FactoryPattern_AbilityWithReflectionStatic GetAbility(string abilityType)
        {
            InitializeFactory();
            
            if (abilitiesByName.ContainsKey(abilityType))
            {
                Type type = abilitiesByName[abilityType];
                var ability = Activator.CreateInstance(type) as FactoryPattern_AbilityWithReflectionStatic;
                return ability;
            }

            return null;
        }

        internal static IEnumerable<string> GetAbilityNames2()
        {
            UnityEngine.Debug.Log("Test");
            InitializeFactory();
            return abilitiesByName.Keys;
        }
    }
}



