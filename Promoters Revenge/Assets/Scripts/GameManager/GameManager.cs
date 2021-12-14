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

        public UIRoot UIRoot;
        public bool IsPaused = false;
        public GameObject LevelExit;

        public GameObject Player;
        public GameObject PlayerSpawnPoint;

        protected void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            LevelExit = GameObject.FindGameObjectWithTag("LevelExit");
            LevelExit.SetActive(false);
        }

        public void StartLevel()
        {
            UIRoot.OnLevelStart();
            PlayerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            Player.TryGetComponent<Rigidbody>(out var playerRigidbody);
            playerRigidbody.useGravity = false;
            Player.transform.position = PlayerSpawnPoint.transform.position;
            Player.transform.rotation = PlayerSpawnPoint.transform.rotation;
            playerRigidbody.useGravity = true;
        }

        protected void Update()
        {
            GamePauseLogic();
        }

        public void OpenLevelExit()
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
            UIRoot.HidePauseMenu();
            Time.timeScale = 1f;
        }

        public void PauseGame()
        {
            IsPaused = true;
            UIRoot.ShowPauseMenu();
            Time.timeScale = 1e-4f;
        }

        protected void OnDestroy()
        {
            Instance = null;
        }
    }
}