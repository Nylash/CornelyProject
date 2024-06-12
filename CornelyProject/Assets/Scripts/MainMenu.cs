using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadDefaultMain()
    {
        PlayerPrefs.SetInt("UseSprite", 0);
        SceneManager.LoadScene("Main");
    }

    public void LoadMain(string path)
    {
        PlayerPrefs.SetInt("UseSprite", 1);
        PlayerPrefs.SetString("SpritePath", path);
        SceneManager.LoadScene("Main");
    }
}
