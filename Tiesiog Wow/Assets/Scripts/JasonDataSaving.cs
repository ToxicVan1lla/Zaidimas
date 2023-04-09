using Newtonsoft.Json;
using System.IO;
using UnityEngine;
//---------------Dabar nenaudojamas---------------------
public class JasonDataSaving : InterfaceDataSaving
{
    public void SaveData<T>(string RelativePath, T Data)
    {
        string path = Application.persistentDataPath + RelativePath;

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using FileStream stream = File.Create(path);
        stream.Close();    
        File.WriteAllText(path, JsonConvert.SerializeObject(Data));            
    }
    public T LoadData<T>(string RelativePath)
    {
        string path = Application.persistentDataPath + RelativePath;

        T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        return data;

    }
        

}
