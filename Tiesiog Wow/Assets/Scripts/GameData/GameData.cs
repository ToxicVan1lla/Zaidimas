using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coins;
    public Vector2 position;
    public string sceneName;
    public int graveValue;
    public string graveScene;
    public bool graveActive;
    public Vector2 gravePosition;
    public bool hasPotions;
    public int numberOfPotions;
    [Header("Boss")]
    public bool cleanerBossAlive;
    public GameData()
    {
        this.graveValue = 0;
        this.graveActive = false;
        this.graveScene = "Room1";
        this.gravePosition = Vector2.zero;
        this.sceneName = "Foje";
        this.coins = 30;
        this.position = new Vector2(4.43f, 0f);
        this.cleanerBossAlive = true;
        this.hasPotions = true;
        this.numberOfPotions = 0;
    }

}

