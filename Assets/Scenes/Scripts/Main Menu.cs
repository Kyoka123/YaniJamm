using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("DiveIn");

    }

    public void LevelsMenu()
    {
        SceneManager.LoadScene("Levels");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
