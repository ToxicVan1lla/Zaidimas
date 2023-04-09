using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    [SerializeField] string sceneToSwitchToName;
    [SerializeField] GameObject transitionScreen;
    [SerializeField] Transform doorLeadsTo;
    private Animator anim;
    private GameObject player;
    [SerializeField] private KeepData keepData;
    private PlayerHealth playerHealth;


    private void Start()
    {
        anim = transitionScreen.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(Transition());
    }
    private IEnumerator Transition()
    {
        keepData.enteredRoom = true;
        player.GetComponent<Movement>().detectInput = false;
        keepData.facingDirection = (int)Mathf.Sign(player.transform.position.x) * 1;
        keepData.health = playerHealth.health;
        anim.SetTrigger("Transition");
        yield return new WaitForSeconds(1f);
        keepData.positionX = doorLeadsTo.position.x;
        keepData.positionY = doorLeadsTo.position.y;
        SceneManager.LoadScene(sceneToSwitchToName);
    }
    
}
