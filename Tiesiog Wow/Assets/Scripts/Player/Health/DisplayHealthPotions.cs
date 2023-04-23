using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHealthPotions : MonoBehaviour, IDataPersistence
{
    [SerializeField] PlayerHealth playerHealth;
    private TextMeshProUGUI displayPotions;
    private int numberOfPotions;
    public void LoadData(GameData data)
    {
        numberOfPotions = data.numberOfPotions;
    }

    public void SaveData(ref GameData data)
    {

    }

    private void Start()
    {
        displayPotions = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        displayPotions.text = numberOfPotions.ToString();
    }

}
