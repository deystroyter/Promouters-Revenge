using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private static UIRoot _instance = null;

    public static UIRoot Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new System.Exception("UIRoot Instance is null!");
            }

            return _instance;
        }
    }

    protected void Awake()
    {
        _instance = this;

        // DontDestroyOnLoad(gameObject);
    }

    [Header("References to GameObjects")] [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _hUD;
    [SerializeField] private GameObject _inputSettings;
    [SerializeField] private UI_AbilitiesPanel _uiAbilitiesPanel;

    public void OnLevelStart()
    {
        ShowPlayerHUD();
        HidePauseMenu();
    }


    public void OpenWindow(object obj)
    {
    }

    public void ShowPlayerHUD()
    {
        _hUD.SetActive(true);
    }

    public void HidePlayerHUD()
    {
        _hUD.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        _pauseMenu.SetActive(false);
    }

    public void ShowSettings()
    {
        _inputSettings.SetActive(true);
    }

    public void HideSettings()
    {
        _inputSettings.SetActive(false);
        _uiAbilitiesPanel.UpdateUIAbilitiesPanel();
    }
}