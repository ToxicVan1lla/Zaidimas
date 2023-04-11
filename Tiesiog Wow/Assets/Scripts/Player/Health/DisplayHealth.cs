using UnityEngine;
using UnityEngine.UI;
public class DisplayHealth : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = playerHealth.health / 10;
    }
    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.health / 10;
    }
}
