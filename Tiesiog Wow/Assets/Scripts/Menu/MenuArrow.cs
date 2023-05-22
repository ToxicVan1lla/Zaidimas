using UnityEngine;
using UnityEngine.UI;
using System;
public class MenuArrow : MonoBehaviour
{
    [SerializeField] private AudioClip swithchSound;
    private RectTransform rect;
    [SerializeField] private RectTransform[] options;
    private int currentOption;
    private float sinceLastChange;
    private float sinceLastChangeTime = 0.3f;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (sinceLastChange > sinceLastChangeTime && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            changeOption(-1);
            sinceLastChange = 0;
        }
        else if (sinceLastChange > sinceLastChangeTime && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            changeOption(1);
            sinceLastChange = 0;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            changeOption(-1);
            sinceLastChange = 0;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            changeOption(1);
            sinceLastChange = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Interact();
        sinceLastChange += Time.unscaledDeltaTime;

    }

    private void changeOption(int changeAmount)
    {
        soundManager.instance.playSound(swithchSound);
        currentOption += changeAmount;
        if (currentOption < 0)
            currentOption = options.Length - 1;
        else if (currentOption > options.Length - 1)
            currentOption = 0;
        rect.position = new Vector3(rect.position.x, options[currentOption].position.y, 0);
    }

    private void Interact()
    {
        options[currentOption].GetComponent<Button>().onClick.Invoke();
    }
    public void hoverOver(RectTransform button)
    {
        soundManager.instance.playSound(swithchSound);
        currentOption = Array.IndexOf(options, button);
        rect.position = new Vector3(rect.position.x, options[currentOption].position.y, 0);
    }
}
