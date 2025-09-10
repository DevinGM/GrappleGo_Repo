using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Script Creator: Songchalee, James
/// Recent Date Modified: 
/// 09 - 08 - 2025
/// 09 - 09 - 2025

public class MenuButtons : MonoBehaviour
{
    //  For PlayButton on MainMenu Scene. //
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // For QuitButton on MainMenu Scene. //
    public void QuitGame()
    {
        Application.Quit();
    }


    public void EnterSetting()
    {
        SceneManager.LoadScene("SettingMenu");
    }

    public void ExitSetting()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
