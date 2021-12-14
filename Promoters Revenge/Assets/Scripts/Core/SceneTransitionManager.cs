using System;
using System.Collections;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance;

        private AsyncOperation _loadingSceneOperation;

        [SerializeField] private Animator _componentAnimator;

        [SerializeField] private Text _sceneInProcessText;
        [SerializeField] private Text _loadingPercentageText;
        [SerializeField] private Image _fadingBackgroundImage;

        public string ActiveScene;

        protected void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        protected void Start()
        {
            StartCoroutine(SwitchToMainMenu());
        }

        protected void Update()
        {
            if (_loadingSceneOperation != null)
            {
                _loadingPercentageText.text = $"Loading ... {Instance._loadingSceneOperation.progress * 100}%";
            }
        }

        private IEnumerator SwitchToLevel(string levelName)
        {
            _loadingSceneOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("_SessionSystems", LoadSceneMode.Single);
            _sceneInProcessText.text = " > SessionSystems <";

            while (!_loadingSceneOperation.isDone)
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(1f);


            _loadingSceneOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            _sceneInProcessText.text = $"> {levelName} <";


            while (!_loadingSceneOperation.isDone)
            {
                yield return new WaitForSeconds(0.5f);
            }


            //var levelInfo = transform.Find("LevelInfo");

            //GameManager.Instance.Linking(levelInfo);
            //GameManager.Instance.InitLevel();


            yield return new WaitForSeconds(1f);
            SceneOpeningLogic();

            //TODO: Waiting for animations end

            GameManager.Instance.StartLevel();
        }

        private IEnumerator SwitchToMainMenu()
        {
            yield return new WaitForSeconds(1f);

            _loadingSceneOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            _sceneInProcessText.text = " > MainMenu <";

            while (!_loadingSceneOperation.isDone)
            {
                yield return new WaitForSeconds(0.5f);
            }


            yield return new WaitForSeconds(3f);

            SceneOpeningLogic();
        }

        public void SwitchToScene(string sceneName)
        {
            ActiveScene = sceneName;
            SceneClosingLogic();

            switch (sceneName)
            {
                case var str when sceneName.Contains("Level"):
                    StartCoroutine(SwitchToLevel(sceneName));
                    return;
                case "MainMenu":
                    StartCoroutine(SwitchToMainMenu());
                    return;
                default:
                    throw new Exception($"{sceneName} ]- unknown SceneName!");
            }
        }

        private void SceneClosingLogic()
        {
            _fadingBackgroundImage.raycastTarget = true;
            _componentAnimator.SetTrigger("sceneClosing");
        }

        private void SceneOpeningLogic()
        {
            _fadingBackgroundImage.raycastTarget = false;
            _componentAnimator.SetTrigger("sceneOpening");
        }

        protected void OnDestroy()
        {
            Instance = null;
        }
    }
}