using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField] GameObject coin;
    private float maxSpeedX = 4;
    private float maxSpeedY = 7;

    public void spawnCoin(Transform position)
    {

        GameObject coinClone = Instantiate(coin, new Vector3(position.position.x, position.position.y, 0), transform.rotation);
        coinClone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-maxSpeedX, maxSpeedX), Random.Range(2, maxSpeedY));
        
    }


}
