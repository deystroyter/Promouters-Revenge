using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PauseMenuController : MonoBehaviour
    {
        public Button ResumeButton;
        public Button RestartButton;
        public Button SettingsButton;
        public Button ExitToMainMenuButton;
        public Button ExitToDesktopButton;

        protected void Start()
        {
            gameObject.SetActive(false);

            ResumeButton.onClick.AddListener(ResumeGame);
            RestartButton.onClick.AddListener(RestartLevel);
            SettingsButton.onClick.AddListener(ShowSettings);
            ExitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
            ExitToDesktopButton.onClick.AddListener(ExitToDesktop);
        }

        public void ResumeGame()
        {
            GameManager.Instance.ResumeGame();
        }

        public void RestartLevel()
        {
            SceneTransitionManager.Instance.SwitchToScene(SceneTransitionManager.Instance.ActiveScene);
            ResumeGame();
        }

        public void ShowSettings()
        {
            UIRoot.Instance.ShowSettings();
        }

        public void ExitToMainMenu()
        {
            SceneTransitionManager.Instance.SwitchToScene("MainMenu");
            ResumeGame();
        }

        public void ExitToDesktop()
        {
            Application.Quit();
        }
    }
}