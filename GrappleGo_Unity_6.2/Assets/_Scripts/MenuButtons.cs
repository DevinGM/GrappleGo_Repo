using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Script Creator: Songchalee, James
/// Recent Date Modified: 
/// 09 - 08 - 2025
/// 09 - 09 - 2025
/// 10 - 07 - 2025

public class MenuButtons : MonoBehaviour
{
    //  For PlayButton on MainMenu Scene. //
    public void PlayGame()
    {
        Debug.Log("Entering Game");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("Devin_Work");
    }

    // For QuitButton on MainMenu Scene. //
    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void EnterShop()
    {
        Debug.Log("Enter Shop");
        SceneManager.LoadScene("Shop");
    }

    public void EnterSetting()
    {
        Debug.Log("Enter Settings");
        SceneManager.LoadScene("SettingMenu");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
