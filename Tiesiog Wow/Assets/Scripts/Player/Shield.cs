using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool shieldAcitve;
    private PlayerAttack attack;
    private Movement playerScript;
    [SerializeField] private float timeAfterHit;
    [SerializeField] private float speedWithShield;
    [SerializeField] private GameObject shield;
    private float afterHitCounter;
    private void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Movement>();
        attack = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }
    private void Update()
    {
        if(playerScript.detectInput)
        {

            if (Input.GetKeyDown(KeyCode.LeftControl) && afterHitCounter < 0 && !attack.isAttacking)
            {
                shield.SetActive(true);
                shieldAcitve = true;
                playerScript.speed = speedWithShield;
                attack.canAttack = false;
            }
            else
                afterHitCounter -= Time.deltaTime;
            if(shieldAcitve && Input.GetKeyUp(KeyCode.LeftControl))
            {
                shield.SetActive(false);
                shieldAcitve = false;
                playerScript.speed = playerScript.defaultSpeed;
                attack.canAttack = true;
            }
        }
    }
    public void deactivateShield()
    {
        shield.SetActive(false);
        shieldAcitve = false;
        afterHitCounter = timeAfterHit;
        playerScript.speed = playerScript.defaultSpeed;
        attack.canAttack = true;
    }

}
