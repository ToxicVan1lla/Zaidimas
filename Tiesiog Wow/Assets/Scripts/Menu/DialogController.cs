using System.Collections;
using TMPro;
using UnityEngine;
public class DialogController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject canvas;
    [SerializeField] private float timeToprint1Letter;
    [SerializeField] private string[] messages;
    private Coroutine coroutine;
    private GameObject player;
    private bool isTalking = false, isTyping = false;
    private int index = 0;
    private float timeUntilSkip;
    [SerializeField] private Animator anim;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        timeUntilSkip -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.E) && !isTalking && Mathf.Abs(player.transform.position.x - transform.position.x) < 3)
        {
            player.GetComponent<Movement>().detectInput = false;
            player.GetComponent<Movement>().horizontalInput = 0;
            canvas.SetActive(true);
            isTalking = true;
            coroutine = StartCoroutine(writeText(index));
            timeUntilSkip = 0.2f;
            if(player.transform.localScale.x > 0 && player.transform.position.x > transform.position.x)
                player.GetComponent<Movement>().Flip();
            if (player.transform.localScale.x < 0 && player.transform.position.x < transform.position.x)
            {
                Debug.Log(transform.position);
                player.GetComponent<Movement>().Flip();
            }
            anim.SetBool("Talk", true);
        }
        if(isTalking && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && timeUntilSkip < 0)
        {
            if(isTyping)
            {
                isTyping = false;
                StopCoroutine(coroutine);
                text.text = messages[index];
            }
            else
            {
                text.text = "";
                index++;
                if (index >= messages.Length)
                {
                    canvas.SetActive(false);
                    index = 0;
                    isTalking = false;
                    player.GetComponent<Movement>().detectInput = true;
                    anim.SetBool("Talk", false);

                }
                else
                    coroutine = StartCoroutine(writeText(index));
            }
        }
    }

    private IEnumerator writeText(int index)
    {
        isTyping = true;
        foreach(char c in messages[index])
        {
            text.text += c;
            yield return new WaitForSeconds(timeToprint1Letter);
        }
        isTyping = false;
    }
}