using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public bool canBlock = true;
    public bool shieldAcitve;
    private PlayerAttack attack;
    private Movement playerScript;
    [SerializeField] private float speedWithShield;
    [SerializeField] private float jumpForceWithShield;
    [SerializeField] private GameObject shield;
    private float afterHitCounter;
    private float activateCounter;
    [SerializeField] private float parryTime;
    [SerializeField] private float dechargeTime;
    [SerializeField] private float rechargeTime;
    public float parryCounter;
    public bool blocked;
    private float blockEnergy = 1;
    private bool isRecharging;
    private bool isDecharging;
    private Image image;
    private void Start()
    {
        image = GameObject.Find("Current").GetComponent<Image>();

        playerScript = GameObject.Find("Player").GetComponent<Movement>();
        attack = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }
    private void Update()
    {
        activateCounter -= Time.deltaTime;
        parryCounter -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1) && canBlock)
            activateCounter = 0.3f;
        image.fillAmount = blockEnergy;
        if(blocked)
        {
            StartCoroutine(changeEnergyAmmount());
            blocked = false;
        }
        if(!isDecharging && !isRecharging && blockEnergy != 1)
        {
            StartCoroutine(changeEnergyAmmount());
        }

        if (playerScript.detectInput)
        {

            if (activateCounter > 0 && afterHitCounter < 0 && !attack.isAttacking && blockEnergy == 1)
            {
                parryCounter = parryTime;
                activateCounter = 0;
                shield.SetActive(true);
                shieldAcitve = true;
                playerScript.speed = speedWithShield;
                playerScript.jumpForce = jumpForceWithShield;
                playerScript.canDash = false;
                attack.canAttack = false;
            }
            else
                afterHitCounter -= Time.deltaTime;
            if(Input.GetKeyUp(KeyCode.Mouse1) && canBlock)
            {
                activateCounter = 0;
                shield.SetActive(false);
                shieldAcitve = false;
                playerScript.speed = playerScript.defaultSpeed;
                playerScript.jumpForce = playerScript.DefaultjumpForce;
                playerScript.canDash = true;
                attack.canAttack = true;
            }
        }
    }
    public void deactivateShield()
    {
        activateCounter = 0;
        shield.SetActive(false);
        shieldAcitve = false;
        playerScript.speed = playerScript.defaultSpeed;
        playerScript.jumpForce = playerScript.DefaultjumpForce;
        playerScript.canDash = true;
        attack.canAttack = true;
    }

    public IEnumerator Parry()
    {
        playerScript.playerAttack.noCooldown = true;
        Time.timeScale = 0.3f;
        playerScript.playerAttack.attackSpeed = 2.3f;
        playerScript.speed = 25;
        yield return new WaitForSeconds(0.4f);
        playerScript.playerAttack.noCooldown = false;
        playerScript.speed = playerScript.defaultSpeed;
        playerScript.playerAttack.attackSpeed = 1;
        Time.timeScale = 1;
    }
    private IEnumerator changeEnergyAmmount()
    {
        float currentTime = 0;
        if(blockEnergy==1)
        {
            isDecharging = true;
            while(currentTime <= dechargeTime)
            {

                blockEnergy = Mathf.Lerp(1, 0, currentTime / dechargeTime);
                currentTime += Time.deltaTime;
                yield return 0;
            }
            blockEnergy = 0;
            isDecharging = false;
        }
        else
        {
            isRecharging = true;
            yield return new WaitForSeconds(0.2f);
            while (currentTime <= rechargeTime)
            {

                blockEnergy = Mathf.Lerp(0, 1, currentTime / rechargeTime);
                currentTime += Time.deltaTime;
                yield return 0;
            }
            blockEnergy = 1;
            isRecharging = false;

        }
    }
}
