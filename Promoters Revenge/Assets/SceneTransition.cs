using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
    public class SceneTransition : MonoBehaviour
    {
        private static SceneTransition _instance;

        private Animator _componentAnimator;

        private AsyncOperation _loadingSceneOperation;

        private static bool shouldPlaySceneOpening = false;

        [SerializeField] private Text loadingPercentage;

        // Start is called before the first frame update
        protected void Start()
        {
            _instance = this;
            _componentAnimator = GetComponent<Animator>();

            if (shouldPlaySceneOpening)
            {
                _componentAnimator.SetTrigger("sceneOpening");
            }
        }

        protected void Update()
        {
            if (_loadingSceneOperation != null)
            {
                loadingPercentage.text = $"Loading ... {_loadingSceneOperation.progress * 100}";
            }
        }

        public void SwitchToScene(string sceneName)
        {
            _instance._componentAnimator.SetTrigger("sceneClosing");
            _instance._loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            _instance._loadingSceneOperation.allowSceneActivation = false;
            Debug.Log("Animation Ñlosing OVER!");
        }


        public void OnAnimationOver()
        {
            shouldPlaySceneOpening = true;
            _instance._loadingSceneOperation.allowSceneActivation = true;
        }
    }
}