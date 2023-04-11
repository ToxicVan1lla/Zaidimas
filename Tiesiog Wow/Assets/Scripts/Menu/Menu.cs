using UnityEngine;

public class Menu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject menu;
    private void Start()
    {
        menu.SetActive(true);
        menu.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();

        }

    }

    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
