// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using DESIGN_PATTERNS.Singleton_Pattern.Singleton_Class;
using DG.Tweening;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Loading_Scene.Scripts
{
    /// <summary>
    /// This is going to act as a director for the states of the game and will as the name suggests
    /// persist between all of the different scene states and manage the logic for moving between them.
    ///
    /// Ref : https://www.youtube.com/watch?v=iXWFTgFNRdM&ab_channel=GameDevGuide
    /// </summary>
    public enum SceneIndexes
    {
        MANAGER = 0,
        MAIN_SCENE = 1,
        MAP = 2,
        NAVMESH = 3
    }

    public class GameAndSceneManager : SingletonBehaviour<GameAndSceneManager>
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private ProgressBar _progressBar;
        [SerializeField] private TextMeshProUGUI _loadingTextField;

        [SerializeField] private Sprite[] _backgrounds;
        [SerializeField] private Image _backgroundImage;
        
        [SerializeField] private TextMeshProUGUI _tipsTextField;
        [SerializeField] private string[] _tips;
        
        private List<AsyncOperation> _scenesLoading = new List<AsyncOperation>();
        private float _totalSceneProgress;

        private CanvasGroup _alphaCanvas;
        private int _tipCount;
        private void Awake()
        {
            SceneManager.LoadSceneAsync((int) SceneIndexes.MAIN_SCENE, LoadSceneMode.Additive);
            _alphaCanvas = _tipsTextField.gameObject.GetComponent<CanvasGroup>();
        }

        public void LoadGame()
        {
            _backgroundImage.sprite = _backgrounds[Random.Range(0, _backgrounds.Length)];
            _loadingScreen.SetActive(true);

            StartCoroutine(GenerateTips());
            _scenesLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndexes.MAIN_SCENE));
            _scenesLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndexes.MAP, LoadSceneMode.Additive));
            _scenesLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndexes.NAVMESH, LoadSceneMode.Additive));

            StartCoroutine(GetSceneLoadProgressCoroutine());
        }

        private IEnumerator GetSceneLoadProgressCoroutine()
        {
            for (int i = 0; i < _scenesLoading.Count; i++)
            {
                while (!_scenesLoading[i].isDone)
                {
                    CalculatePercentage();
                    yield return null;
                }
            }
            
            _loadingScreen.SetActive(false);
        }

        private IEnumerator GenerateTips()
        {
            _tipCount = Random.Range(0, _tips.Length);
            _tipsTextField.text = _tips[_tipCount];

            while (_loadingScreen.activeInHierarchy)
            {
                yield return new WaitForSeconds(3f);
                _alphaCanvas.alpha = 0f;
                _alphaCanvas.DOFade(1,0.5f);
                yield return new WaitForSeconds(0.5f);
                _tipCount++;
                if (_tipCount >= _tips.Length)
                {
                    _tipCount = 0;
                }

                _tipsTextField.text = _tips[_tipCount];

                _alphaCanvas.DOFade(0, 0.5f);
            }
        }

        private void CalculatePercentage()
        {
            _totalSceneProgress = 0;
            foreach (AsyncOperation operation in _scenesLoading)
            {
                _totalSceneProgress += operation.progress;
            }

            _totalSceneProgress = (_totalSceneProgress / _scenesLoading.Count) * 100f;

            SetLoadingText(_totalSceneProgress);
            
            _progressBar.value = Mathf.RoundToInt(_totalSceneProgress);

        }

        private void SetLoadingText(float percentage)
        {
            _loadingTextField.text = string.Format("Loading Environments: {0}%", percentage);
        }
    }
}