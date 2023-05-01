using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHealthPotions : MonoBehaviour, IDataPersistence
{
    [SerializeField] PlayerHealth playerHealth;
    private TextMeshProUGUI displayPotions;
    private int numberOfPotions;
    private bool bought = false;
    private bool paimti = true;
    private DataPersistanceManager manager;


    public void LoadData(GameData data)
    {
        if (paimti)
        {
            numberOfPotions = data.numberOfPotions;
            paimti = false;
        }
    }
    public void SaveData(ref GameData data)
    {
        if (bought)
        {
            data.numberOfPotions = numberOfPotions;
            bought = false;
        }
    }

    private void Start()
    {
        displayPotions = gameObject.GetComponent<TextMeshProUGUI>();
        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
    }

    private void Update()
    {
        displayPotions.text = numberOfPotions.ToString();
    }
    public void addPotions(int amount)
    {
        numberOfPotions += amount;
        bought = true;
        manager.save = true;
    }

}
