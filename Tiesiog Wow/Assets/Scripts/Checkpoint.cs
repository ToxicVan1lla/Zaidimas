using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] KeepData keepData;
    [SerializeField] PlayerHealth playerHealth;
    private bool onCheckpoint = false;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && onCheckpoint)
        {
            keepData.enteredRoom = false;
            keepData.checkpointSceneName = SceneManager.GetActiveScene().name;
            keepData.checkpointX = gameObject.transform.position.x;
            keepData.checkpointY = gameObject.transform.position.y;
            keepData.health = playerHealth.maxHealth;
            playerHealth.health = playerHealth.maxHealth;
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
