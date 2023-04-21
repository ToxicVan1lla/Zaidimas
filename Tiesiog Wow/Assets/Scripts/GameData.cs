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
    [Header("Boss")]
    public bool cleanerBossAlive;
    public GameData()
    {
        this.graveValue = 0;
        this.graveActive = false;
        this.graveScene = "Room1";
        this.gravePosition = Vector2.zero;
        this.sceneName = "Room1";
        this.coins = 0;
        this.position = new Vector2(-7.03f, -2.76f);
        this.cleanerBossAlive = true;
    }

}

