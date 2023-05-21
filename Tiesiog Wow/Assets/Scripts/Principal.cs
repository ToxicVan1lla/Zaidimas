using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Principal : MonoBehaviour, IDataPersistence
{
    private bool principalActive;
    public void LoadData(GameData data)
    {
        principalActive = data.principalActive;
    }

    public void SaveData(ref GameData data)
    {
        
    }
    private void Update()
    {
        if (!principalActive)
            Destroy(gameObject);
    }
}
