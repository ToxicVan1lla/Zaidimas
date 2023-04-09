
//---------------Dabar nenaudojamas---------------------

public interface InterfaceDataSaving
{
    void SaveData<T>(string RelativePath, T Data);

    T LoadData<T>(string RelativePath);
}
