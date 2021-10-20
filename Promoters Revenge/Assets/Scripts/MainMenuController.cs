using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private CinemachineCameraSwitcher cameraSwitcher;

        [SerializeField] private GameObject _character;
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private SceneTransitionManager _sceneTransitionManager;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void GoToLevelsCanvas()
        {
            cameraSwitcher.SwitchToLevels();
            _characterAnimator.SetTrigger("GoToLevels");
        }

        public void GoToMainFromLevelsCanvas()
        {
            cameraSwitcher.SwitchToMainMenu();
            _characterAnimator.SetTrigger("GoToMainFromLevels");
        }

        public void GoToMainFromSettings()
        {
            cameraSwitcher.SwitchToMainMenu();
            _characterAnimator.SetTrigger("GoToMainFromSettings");
        }

        public void GoToSettingsCanvas()
        {
            cameraSwitcher.SwitchToSettings();
            _characterAnimator.SetTrigger("GoToSettings");
        }

        public void SwitchToScene(string sceneName)
        {
            _characterAnimator.SetTrigger("LevelPicked");
            SceneTransitionManager.Instance.SwitchToScene(sceneName);
        }
    }
}