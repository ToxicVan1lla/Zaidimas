using UnityEngine;

[CreateAssetMenu(fileName = "KeepData", menuName = "Data")]
public class KeepData : ScriptableObject
{
    public float positionX = -7.03f, positionY = -2.76f;
    public float health = 3;
    public bool enteredRoom;
    public int facingDirection = 1;
    public string checkpointSceneName;
    public float checkpointX = -7.03f, checkpointY = -2.76f;
    public int coinAmount;
    public bool graveActive = false;
    public string graveScene;
    public float graveX, graveY;
    public int graveValue;
    public int healNumber;
    public bool findingEnemys;
    public float volumeValue;

    private void OnEnable()
    { 
        positionX = checkpointX = -7.03f;
        positionY = checkpointY = -2.76f;
        health = 5;
        facingDirection = 1;
        enteredRoom = false;
        checkpointSceneName = "Room1";
        coinAmount = 0;
        healNumber = 0;
        volumeValue = 1;

}
}
