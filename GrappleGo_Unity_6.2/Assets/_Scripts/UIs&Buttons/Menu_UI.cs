using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Script Creator: Songchalee, James
/// Recent Date Modified: 
/// 10 - 08 - 2025

public class Menu_UI : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Starting Level");
        //SceneManager.LoadScene("Devin_Work");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void EnterShop()
    {
        Debug.Log("Enter Shop");
        //SceneManager.LoadScene("Devin_Shop");
        SceneManager.LoadScene(3);
    }

    public void EnterSetting()
    {
        Debug.Log("Enter Settings");
        //SceneManager.LoadScene("SettingMenu");
        SceneManager.LoadScene(4);
    }

    public void ReturnToMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene(0);
    }
}