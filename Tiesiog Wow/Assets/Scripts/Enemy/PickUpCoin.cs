using UnityEngine;

public class PickUpCoin : MonoBehaviour
{
    private GameObject coinAmount;
    private PlayerCoins playerCoins;
    private bool coinCollected = false;

    private void Start()
    {
        coinAmount = GameObject.Find("CoinAmount");
        playerCoins = coinAmount.GetComponent<PlayerCoins>();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !coinCollected)
        {
            coinCollected = true;
            playerCoins.addCoins(1);
            Destroy(gameObject);
        }
    }
 
}
