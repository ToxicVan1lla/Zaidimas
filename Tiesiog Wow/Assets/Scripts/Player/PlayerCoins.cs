using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerCoins : MonoBehaviour
{
    [SerializeField] KeepData keepData;
    private TextMeshProUGUI displayCoins;
    private TextMeshProUGUI displayCollectedCoins;
    public int coinAmount;
    private int coinsCollected;
    private float transferTime = 1;
    private float transferCounter;
    private GameObject text;
    private void Awake()
    {
        displayCoins = gameObject.GetComponent<TextMeshProUGUI>();
        text = GameObject.Find("PickedUpCoins");
        displayCollectedCoins = text.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        coinsCollected = 0;
        coinAmount = keepData.coinAmount;
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
            keepData.coinAmount += amount;
            displayCollectedCoins.text = "+" + coinsCollected.ToString();
        }
    }
    private IEnumerator Transfer()
    {
        for (int i=1;i<=coinsCollected;i++)
        {
            coinAmount++;
            displayCoins.text = coinAmount.ToString();
            displayCollectedCoins.text = "+" + (coinsCollected - i).ToString();
            yield return new WaitForSeconds(0.5f / coinsCollected);
        }
        text.SetActive(false);
        coinsCollected = 0;
    }

}
