// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagerAndLoadingScreen.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton

        public static LevelManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                {
                    Destroy(gameObject);
                }
            }
        }

        #endregion
        
        [SerializeField] private GameObject _loaderCanvas;
        [SerializeField] private Image _progressBar; // Gotta be Filled, Horizontal and Left
        [Space]
        [SerializeField] private bool _smoothLoadingBar = false;

        private float _target;

        public async void LoadScene(string sceneName)
        {
            _target = 0;
            _progressBar.fillAmount = 0;
            var scene = SceneManager.LoadSceneAsync(sceneName);
            
            _loaderCanvas.SetActive(true);

            do
            {
                await Task.Delay(100);
                _progressBar.fillAmount = scene.progress;
            } while (scene.progress <0.9f);

            scene.allowSceneActivation = true;
            _loaderCanvas.SetActive(false);
        }

        public void Update()
        {
            if(!_smoothLoadingBar) return;
            
            _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
        }
    }
}
