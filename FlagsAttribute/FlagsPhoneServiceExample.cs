// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace FlagsAttribute
{
    /// <summary>
    /// "|" means or,
    /// "&" means and,
    /// "==" same,
    /// 
    /// Ref : https://learn.microsoft.com/tr-tr/dotnet/api/system.flagsattribute?view=net-6.0
    /// </summary>
    
    [System.Flags]
    public enum PhoneService
    {
        None = 0,
        LandLine = 1 << 0,
        Cell = 1 << 2,
        Fax = 1 << 3,
        Internet = 1 << 4,
        Other = 1 << 8
    }
    
    public class FlagsPhoneServiceExample : MonoBehaviour
    {
        private PhoneService _household1;
        private PhoneService _household2;
        private PhoneService _household3;

        private PhoneService[] _households;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            CheckConditions();
        }

        private void Initialize()
        {
            _household1 = PhoneService.LandLine | PhoneService.Cell | PhoneService.Internet;
            _household2 = PhoneService.None;
            _household3 = PhoneService.Cell | PhoneService.Internet;
            
            _households = new PhoneService[]{_household1,_household2,_household3};

        }
        private void CheckConditions()
        {
            // Which households have no service?
            for (int ctr = 0; ctr < _households.Length; ctr++)
                Debug.LogFormat("Household {0} has phone service: {1}",
                    ctr + 1,
                    _households[ctr] == PhoneService.None ?
                        "No" : "Yes");
            Debug.LogFormat("\n");

            // Which households have cell phone service?
            for (int ctr = 0; ctr < _households.Length; ctr++)
                Debug.LogFormat("Household {0} has cell phone service: {1}",
                    ctr + 1,
                    (_households[ctr] & PhoneService.Cell) == PhoneService.Cell ?
                        "Yes" : "No");
            Debug.LogFormat("\n");

            // Which households have cell phones and land lines?
            var cellAndLand = PhoneService.Cell | PhoneService.LandLine;
            for (int ctr = 0; ctr < _households.Length; ctr++)
                Debug.LogFormat("Household {0} has cell and land line service: {1}",
                    ctr + 1,
                    (_households[ctr] & cellAndLand) == cellAndLand ?
                        "Yes" : "No");
            Debug.LogFormat("\n");

            // List all types of service of each household?//
            for (int ctr = 0; ctr < _households.Length; ctr++)
                Debug.LogFormat("Household {0} has: {1:G}",
                    ctr + 1, _households[ctr]);
            Debug.LogFormat("\n");
        }
    }
}
