using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Scripts.UI.HUD;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameObject PauseMenu;
        public bool IsPaused = false;

        protected void Awake()
        {
            Instance = this;
        }

        public void StartLevel()
        {
            HUDManager.Instance.gameObject.SetActive(true);
        }

        protected void Update()
        {
            GamePauseLogic();
        }

        public void OpenLevelExit(GameObject LevelExit)
        {
            LevelExit.SetActive(true);
        }

        public void ExitLevel()
        {
            SceneTransitionManager.Instance.SwitchToScene("MainMenu");
        }

        private void GamePauseLogic()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            IsPaused = false;
            PauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void PauseGame()
        {
            IsPaused = true;
            PauseMenu.SetActive(true);
            Time.timeScale = 1e-4f;
        }

        protected void OnDestroy()
        {
            Instance = null;
        }
    }
}