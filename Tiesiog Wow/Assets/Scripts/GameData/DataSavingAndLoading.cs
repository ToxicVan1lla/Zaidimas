using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class DataSavingAndLoading
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public DataSavingAndLoading(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    public void Save(GameData Data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonConvert.SerializeObject(Data, Formatting.Indented);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch(Exception e)
        {
            Debug.Log(e.Message + " " + e.StackTrace);
        }

    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {

                loadedData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(fullPath));
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                throw e;
            }

        }
        return loadedData;
    }
}
