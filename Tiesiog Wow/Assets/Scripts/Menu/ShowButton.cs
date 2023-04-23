using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowButton : MonoBehaviour
{
    [SerializeField] GameObject buttonIcon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            buttonIcon.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            buttonIcon.SetActive(false);
    }
}
