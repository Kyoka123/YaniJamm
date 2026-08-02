using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");

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
