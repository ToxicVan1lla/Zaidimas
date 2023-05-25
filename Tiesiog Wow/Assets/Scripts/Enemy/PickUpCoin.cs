using UnityEngine;

public class PickUpCoin : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;
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
            if(soundManager.instance.Playing < 4)
                soundManager.instance.playSound(pickupSound);
            coinCollected = true;
            playerCoins.addCoins(1);
            Destroy(gameObject);
        }
    }
 
}
