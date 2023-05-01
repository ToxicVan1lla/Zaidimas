using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerCoins : MonoBehaviour, IDataPersistence
{
    private TextMeshProUGUI displayCoins;
    private TextMeshProUGUI displayCollectedCoins;
    public int coinAmount;
    public int coinsCollected;
    private float transferTime = 1;
    private float transferCounter;
    private GameObject text;
    private int coinsLeft = 0;

    public void LoadData(GameData data)
    {
        this.coinAmount = data.coins;
        displayCoins.text = coinAmount.ToString();

    }

    public void SaveData(ref GameData data)
    {
        data.coins = (int)Mathf.Clamp(coinAmount + coinsLeft, 0, Mathf.Infinity);
    }

    private void Awake()
    {
        displayCoins = gameObject.GetComponent<TextMeshProUGUI>();
        text = GameObject.Find("PickedUpCoins");
        displayCollectedCoins = text.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        coinsCollected = 0;
        displayCoins.text = coinAmount.ToString();
    }
    private void Update()
    {
        transferCounter += Time.deltaTime;
        if (coinsCollected > 0 && transferCounter > transferTime)
        {
            transferCounter = 0;
            StartCoroutine(Transfer());
        }
    }

    public void addCoins(int amount)
    {
        if(amount != 0)
        {
            text.SetActive(true);
            transferCounter = 0;
            coinsCollected += amount;
            displayCollectedCoins.text = "+" + coinsCollected.ToString();
        }
    }
    private IEnumerator Transfer()
    {
        coinsLeft = coinsCollected;
        for (int i=1;i<=coinsCollected;i++)
        {
            coinAmount++;
            coinsLeft--;
            displayCoins.text = coinAmount.ToString();
            displayCollectedCoins.text = "+" + (coinsCollected - i).ToString();
            yield return new WaitForSeconds(0.5f / coinsCollected);
        }
        text.SetActive(false);
        coinsCollected = 0;
    }

}
