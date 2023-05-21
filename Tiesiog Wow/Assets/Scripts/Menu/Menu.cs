using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour, IDataPersistence
{
    public static bool gameIsPaused = false;
    public GameObject menu;
    public bool startMenu;
    private DataPersistanceManager manager;
    private bool startNewGame = false;
    [SerializeField] PlayerCoins coins;
    [SerializeField] DisplayHealthPotions potions;
    [SerializeField] KeepData keepData;
    private bool NewGame = true;
    public void LoadData(GameData data)
    {
        NewGame = data.newGame;
    }
    public void SaveData(ref GameData data)
    {
        if(startNewGame)
        {
            data.enemies.Clear();
            data.gameTime = 0;
            data.newGame = true;
            keepData.enteredRoom = false;
            coins.coinAmount = 30;
            data.graveValue = 0;
            data.graveActive = false;
            data.graveScene = "Room1";
            data.gravePositionX = 0;
            data.gravePositionY = 0;
            data.sceneName = "Foje";
            data.coins = 30;
            data.positionX = 4.43f;
            data.positionY = -1.5f;
            data.cleanerBossAlive = true;
            data.hasPotions = true;
            data.numberOfPotions = 0;
            data.teachBlock = true;
            data.teachAttack = true;
            data.teachHeal = true;
            data.principalActive = true;
            Debug.Log("Naujas");

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
;
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
        manager.save = true;                    
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
