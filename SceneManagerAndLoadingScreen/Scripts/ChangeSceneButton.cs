// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace SceneManagerAndLoadingScreen.Scripts
{
    public class ChangeSceneButton : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            LevelManager.Instance.LoadScene(sceneName);
        }

    }
}
