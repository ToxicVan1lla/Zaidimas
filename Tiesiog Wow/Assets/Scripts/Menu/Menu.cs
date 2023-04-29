using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour,IDataPersistence
{
    public static bool gameIsPaused = false;
    public GameObject menu;
    public bool startMenu;
    private DataPersistanceManager manager;
    private bool startNewGame = false;
    [SerializeField] PlayerCoins coins;
    [SerializeField] DisplayHealthPotions potions;
    [SerializeField] KeepData keepData;

    public void LoadData(GameData data)
    {

    }
    public void SaveData(ref GameData data)
    {
        if(startNewGame)
        {
            keepData.enteredRoom = false;
            coins.coinAmount = 30;
            data.graveValue = 0;
            data.graveActive = false;
            data.graveScene = "Room1";
            data.gravePosition = Vector2.zero;
            data.sceneName = "Foje";
            data.coins = 30;
            data.position =  new Vector2(4.43f, 0f);
            data.cleanerBossAlive = true;
            data.hasPotions = true;
            data.numberOfPotions = 0;

        }
    }
    private void Start()
    {
        if(!startMenu)
        {
            menu.SetActive(true);
            menu.SetActive(false);
            manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
        }
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
        keepData.enteredRoom = false;
        keepData.health = 5;
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

    public void returnToStartMenu()
    {
        Resume();
        SceneManager.LoadScene("Start_Menu");
    }

    public void newGame()
    {
        startNewGame = true;
        manager.save = true;
        returnToStartMenu();
    }
}
