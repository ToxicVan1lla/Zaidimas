using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour, IDataPersistence
{
    [SerializeField] private AudioClip clickSound;
    public static bool gameIsPaused = false;
    public GameObject menu;
    public bool startMenu;
    private DataPersistanceManager manager;
    private bool startNewGame = false;
    [SerializeField] PlayerCoins coins;
    [SerializeField] DisplayHealthPotions potions;
    [SerializeField] KeepData keepData;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject instructions;
    private bool NewGame = true;
    private bool optionsActive;
    private float gameSpeed = 1;
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

        }
    }
    private void Start()
    {
        if(!startMenu)
        {
            menu.SetActive(true);
            menu.SetActive(false);
        }
            manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();

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
        if (SceneManager.GetActiveScene().name == "Start_Menu")
            return;
        soundManager.instance.playSound(clickSound);
        if(optionsActive)
        {
            
            Options.SetActive(false);
            instructions.SetActive(false);
            optionsActive = false;
            menu.SetActive(true);
            gameIsPaused = true;
            return;
        }
        menu.SetActive(false);
        Time.timeScale = gameSpeed;
        gameIsPaused = false;
    }
    public void StartGame()
    {
        soundManager.instance.playSound(clickSound);
        keepData.enteredRoom = false;
        keepData.health = 5;
        SceneManager.LoadScene("Foje");

    }
    public void Pause()
    {
        if (SceneManager.GetActiveScene().name == "Start_Menu")
            return;
        gameSpeed = Time.timeScale;
        menu.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }
    public void Quit()
    {
        soundManager.instance.playSound(clickSound);
        Application.Quit();
    }

    public void returnToStartMenu()
    {
        soundManager.instance.playSound(clickSound);
        manager.save = true;                    
        Resume();
        SceneManager.LoadScene("Start_Menu");
    }

    public void newGame()
    {
        soundManager.instance.playSound(clickSound);
        startNewGame = true;
        manager.save = true;
        StartGame();
    }

    public void options()
    {
        optionsActive = true;
        soundManager.instance.playSound(clickSound);
        Options.SetActive(true);
        menu.SetActive(false);

    }
    public void deactivateOptions()
    {
        optionsActive = false;
        soundManager.instance.playSound(clickSound);
        menu.SetActive(true);
        instructions.SetActive(false);
        Options.SetActive(false);
    }

    public void finishGame()
    {
        soundManager.instance.playSound(clickSound);
        startNewGame = true;
        manager.save = true;
        SceneManager.LoadScene("Start_Menu");
    }

    public void openInstructions()
    {
        soundManager.instance.playSound(clickSound);
        instructions.SetActive(true);
        menu.SetActive(false);
        optionsActive = true;
    }
}
