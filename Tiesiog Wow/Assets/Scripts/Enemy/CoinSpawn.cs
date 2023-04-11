using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField] GameObject coin;
    private float maxSpeedX = 4;
    private float maxSpeedY = 7;

    public void spawnCoin(Transform position)
    {
        Physics2D.IgnoreLayerCollision(9, 9, true);
        Physics2D.IgnoreLayerCollision(9, 8, true);
        Physics2D.IgnoreLayerCollision(9, 7, true);
        GameObject coinClone = Instantiate(coin, new Vector3(position.position.x, position.position.y, 0), transform.rotation);
        coinClone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-maxSpeedX, maxSpeedX), Random.Range(2, maxSpeedY));
        
    }


}
