using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour, IDataPersistence
{
    [SerializeField] KeepData keepData;
    private bool onCheckpoint = false;
    private DataPersistanceManager manager;
    private bool interacted;

    public void LoadData(GameData data)
    {
    }

    public void SaveData(ref GameData data)
    {
        if(interacted)
        {
            data.sceneName = SceneManager.GetActiveScene().name;
            data.positionX = transform.position.x;
            data.positionY = transform.position.y;
            interacted = false;
        }
    }

    private void Start()
    {
        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && onCheckpoint)
        {
            keepData.enteredRoom = false;
            interacted = true;
            manager.save = true;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onCheckpoint = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onCheckpoint = false;
    }
}
