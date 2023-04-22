using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    // Update is called once per frame
    private void Start()
    {
        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
        anim = transitionScreen.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && StandingOnDoor) {
            StartCoroutine(Transition());

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StandingOnDoor = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StandingOnDoor = false;
    } 
    private IEnumerator Transition()
    {
        keepData.enteredRoom = true;
        player.GetComponent<Movement>().detectInput = false;
        //keepData.facingDirection = (int)Mathf.Sign(player.transform.position.x) * 1;
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


