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
        Debug.Log("Entering Game");
        SceneManager.LoadScene("Devin_Work");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void EnterShop()
    {
        Debug.Log("Enter Shop");
        SceneManager.LoadScene("Devin_Shop");
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
