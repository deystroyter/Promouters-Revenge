using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PauseMenuController : MonoBehaviour
    {
        protected void Start()
        {
            gameObject.SetActive(false);
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