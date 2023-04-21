using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DataPersistanceManager : MonoBehaviour
{
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistences;
    private DataSavingAndLoading dataSavingAndLoading;
    public static DataPersistanceManager instance;
    public bool save = false;
    public bool load = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.dataSavingAndLoading = new DataSavingAndLoading(Application.persistentDataPath, fileName);
        this.dataPersistences = FindAllDataPersistences();
        Load();
    }
    private void Update()
    {
        if (save)
        {
            Save();
            save = false;
        }
        if(load)
        {
            Load();
            load = false;
        }

    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void Load()
    {
        this.gameData = dataSavingAndLoading.Load();
        if(this.gameData == null)
        {
            NewGame();
        }
        foreach(IDataPersistence dataPersistence in dataPersistences)
        {
            dataPersistence.LoadData(gameData);
        }
    }
    
    public void Save()
    {
        foreach (IDataPersistence dataPersistence in dataPersistences)
        {
            dataPersistence.SaveData(ref gameData);
        }
        dataSavingAndLoading.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private List<IDataPersistence> FindAllDataPersistences()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().
            OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistences);
    }
}
