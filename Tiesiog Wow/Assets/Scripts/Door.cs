using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    [SerializeField] string sceneToSwitchToName;
    [SerializeField] GameObject transitionScreen;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(Transition());
    }
    private IEnumerator Transition()
    {
        transitionScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        transitionScreen.SetActive(false);
        SceneManager.LoadScene(sceneName: sceneToSwitchToName);
    }
}
