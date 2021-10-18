using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    private bool MainMenuCamera = true;
    private Animator anim;

    protected void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        anim = GetComponent<Animator>();
    }

    public void SwitchToMainMenu()
    {
        anim.Play("MainMenu_VCamera");
    }

    public void SwitchToLevels()
    {
        anim.Play("Levels_VCamera");
        //#TODO: ќжидание конца анимации
    }

    public void SwitchToSettings()
    {
        anim.Play("Settings_VCamera");
    }
}