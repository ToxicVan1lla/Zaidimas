using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coins;
    public float positionX;
    public float positionY;
    public string sceneName;
    public int graveValue;
    public string graveScene;
    public bool graveActive;
    public float gravePositionX;
    public float gravePositionY;
    public bool hasPotions;
    public int numberOfPotions;
    public bool cleanerBossAlive;
    public bool newGame;
    public float gameTime;
    public Dictionary<string, float> enemies;
    public bool teachAttack, teachBlock, teachHeal;
    public bool principalActive;
    public GameData()
    {
        this.gameTime = 0;
        this.enemies = new Dictionary<string, float>();
        this.newGame = true;
        this.graveValue = 0;
        this.graveActive = false;
        this.graveScene = "Room1";
        this.sceneName = "Foje";
        this.gravePositionX = 0;
        this.gravePositionY = 0;
        this.coins = 30;
        this.positionX = 4.43f;
        this.positionY = -1.5f;
        this.cleanerBossAlive = true;
        this.hasPotions = true;
        this.numberOfPotions = 0;
        this.teachAttack = this.teachBlock = teachHeal = true;
        this.principalActive = true;
    }

}

