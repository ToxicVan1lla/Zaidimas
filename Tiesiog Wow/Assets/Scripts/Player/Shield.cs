using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool shieldAcitve;
    private PlayerAttack attack;
    private Movement playerScript;
    [SerializeField] private float speedWithShield;
    [SerializeField] private float jumpForceWithShield;
    [SerializeField] private GameObject shield;
    private float afterHitCounter;
    private float activateCounter;
    [SerializeField] private float parryTime;
    public float parryCounter;
    private void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Movement>();
        attack = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }
    private void Update()
    {
        activateCounter -= Time.deltaTime;
        parryCounter -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1))
            activateCounter = 0.3f;
        if (playerScript.detectInput)
        {

            if (activateCounter > 0 && afterHitCounter < 0 && !attack.isAttacking)
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
            if(Input.GetKeyUp(KeyCode.Mouse1))
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

}
