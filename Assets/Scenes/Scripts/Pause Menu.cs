using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    void Start()
    {
        container.SetActive(false);
    }
    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
                container.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                container.SetActive(false);
            }
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        container.SetActive(false);
    }

    public void MainMenuButton()
    {
               Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
