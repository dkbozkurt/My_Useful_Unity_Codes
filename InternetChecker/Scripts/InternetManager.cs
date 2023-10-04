// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace InternetChecker.Scripts
{
    public class InternetManager : MonoBehaviour
    {
        [SerializeField] private InternetChecker InternetChecker;
        
        private async void Start()
        {
            var isOnline = await InternetChecker.CheckNetwork();
            Debug.Log($"Is online: {isOnline}");
        }
    }
}
