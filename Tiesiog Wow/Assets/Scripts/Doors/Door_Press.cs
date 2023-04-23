using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_Press : MonoBehaviour
{
    private bool StandingOnDoor;
    [SerializeField] string sceneToSwitchToName;
    [SerializeField] GameObject transitionScreen;
    [SerializeField] Transform doorLeadsTo;
    [SerializeField] public int directionAfterEntering;
    private Animator anim;
    private GameObject player;
    [SerializeField] private KeepData keepData;
    private PlayerHealth playerHealth;
    private DataPersistanceManager manager;
    private void Start()
    {
        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
        anim = transitionScreen.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && StandingOnDoor) {
            StartCoroutine(Transition());

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            StandingOnDoor = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            StandingOnDoor = false;
    } 
    private IEnumerator Transition()
    {
        keepData.enteredRoom = true;
        player.GetComponent<Movement>().detectInput = false;
        keepData.facingDirection = directionAfterEntering;
        keepData.health = playerHealth.health;
        anim.SetTrigger("Transition");
        manager.save = true;
        yield return new WaitForSeconds(1f);
        keepData.positionX = doorLeadsTo.position.x;
        keepData.positionY = doorLeadsTo.position.y;
        SceneManager.LoadScene(sceneToSwitchToName);
    }

}   


