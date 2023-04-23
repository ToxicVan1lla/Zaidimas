using UnityEngine;

public class BuyHeals : MonoBehaviour, IDataPersistence
{
    private int coinAmount;
    [SerializeField] private int potionCost;
    [SerializeField] private DialogController dialog;
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private GameObject text;
    private DataPersistanceManager manager;
    private bool bought;
    
    public void LoadData(GameData data)
    {
    }

    public void SaveData(ref GameData data)
    {
        if(bought)
        {
            data.numberOfPotions++;
            bought = false;
        }
    }
    private void Start()
    {
        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
    }

    public void Buy()
    {
        if (playerCoins.coinAmount >= potionCost)
        {
            playerCoins.coinAmount -= potionCost;
            bought = true;
            manager.save = true;
            manager.load = true;
        }
        else
            text.SetActive(true);
    }
    public void DontBuy()
    {
        dialog.deactivateButtons();
        text.SetActive(false);
    }
}
