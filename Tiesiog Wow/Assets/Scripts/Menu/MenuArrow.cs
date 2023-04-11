using UnityEngine;
using UnityEngine.UI;
using System;
public class MenuArrow : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private RectTransform[] options;
    private int currentOption;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            changeOption(-1);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            changeOption(1);
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Interact();
    }

    private void changeOption(int changeAmount)
    {
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
        currentOption = Array.IndexOf(options, button);
        rect.position = new Vector3(rect.position.x, options[currentOption].position.y, 0);
    }
}
