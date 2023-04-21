using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    [SerializeField] public float maxHealth;
    public float health;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private KeepData keepData;
    private bool invulnerable;
    private Movement movement;
    private DataPersistanceManager manager;
    private bool Died = false;
    private bool switchScene = false;
    private PlayerCoins playerCoins;
    public void SaveData(ref GameData data)
    {
        if(Died)
        {
            data.graveValue = playerCoins.coinAmount + playerCoins.coinsCollected;
            playerCoins.coinAmount = 0;
            playerCoins.coinsCollected = 0;
            data.coins = 0;
            data.gravePosition = new Vector2(movement.lastGroundedX, movement.lastGroundedY);
            data.graveActive = true;
            data.graveScene = SceneManager.GetActiveScene().name;
            Died = false;
            switchScene = true;
        }
    }
    public void LoadData(GameData data)
    {
        if(switchScene)
        {
            switchScene = false;
            SceneManager.LoadScene(data.sceneName);
        }
    }

    void Start()
    {
        playerCoins = GameObject.Find("CoinAmount").GetComponent<PlayerCoins>();
        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
        movement = gameObject.GetComponent<Movement>();
        Physics2D.IgnoreLayerCollision(7, 8, false);
        health = maxHealth;
        health = keepData.health;
        invulnerable = false;

    }
    public void takeDamagePlayer(float _damage)
    {
        if (!invulnerable)
        {
            health = Mathf.Clamp(health - _damage, 0, maxHealth);
            if (health == 0)
            {

                keepData.health = maxHealth;
                keepData.enteredRoom = false;
                Died = true;
                manager.save = true;
                manager.load = true;

            }
            else
                StartCoroutine(invulnerability());

        }
    }

    private IEnumerator invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
        invulnerable = false;

    }
}
