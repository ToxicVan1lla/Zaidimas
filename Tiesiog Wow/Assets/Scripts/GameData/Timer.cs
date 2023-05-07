using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour, IDataPersistence
{
    public float timer;
    private bool load = true;
    public void LoadData(GameData data)
    {
        if(load)
        {
            timer = data.gameTime;
            load = false;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.gameTime = timer;
    }
    void Update()
    {
        timer += Time.deltaTime;
    }
}
