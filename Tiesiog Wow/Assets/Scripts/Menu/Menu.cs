using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour,IDataPersistence
{
    public static bool gameIsPaused = false;
    public GameObject menu;
    private string newscene;
    public void LoadData(GameData data)
    {
        newscene = data.sceneName;
    }
    public void SaveData(ref GameData data)
    {
       
    }
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
    public void StartGame()
    {
        SceneManager.LoadScene("Foje");
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
