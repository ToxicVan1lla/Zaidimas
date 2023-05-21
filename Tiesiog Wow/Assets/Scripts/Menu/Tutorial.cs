using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour, IDataPersistence
{
    private bool teachBlock, teachAttack, teachHeal;
    private GameObject player;
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerBody;
    [SerializeField] private float distanceToShowAttack;
    [SerializeField] private GameObject BlockMessage, AttackMessage, HealMessage;
    private bool active;
    private Shield shield;
    private PlayerAttack attack;
    private bool getInfo = true;
    private Movement movement;
    private PlayerHealth health;
    public void LoadData(GameData data)
    {
        if(getInfo)
        {
            teachHeal = data.teachHeal;
            teachBlock = data.teachBlock;
            teachAttack = data.teachAttack;
            getInfo = false;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.teachHeal = teachHeal;
        data.teachAttack = teachAttack;
        data.teachBlock = teachBlock;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        playerBody = player.GetComponent<Rigidbody2D>();
        shield = player.GetComponent<Shield>();
        attack = player.GetComponent<PlayerAttack>();
        movement = player.GetComponent<Movement>();
        health = player.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        if(teachHeal)
        {
            if(health.health <= 2 && health.numberOfPotions > 0)
            {
                active = true;
                HealMessage.SetActive(true);
            }
            if(health.heal)
            {
                teachHeal = false;
                if(active)
                    HealMessage.SetActive(false);
            }
        }

        if(teachBlock)
        {
            if (shield.shieldAcitve)
            {
                teachBlock = false;
                if(active)
                {
                    movement.canTurn = true;
                    active = false;
                    Time.timeScale = 1;
                    BlockMessage.SetActive(false);
                }
            }
        }
        if(teachAttack)
        {
            if (disntaceToEnnemy() <= distanceToShowAttack && !active)
            {
                AttackMessage.SetActive(true);
                active = true;
            }
            if(active && disntaceToEnnemy() > distanceToShowAttack)
            {
                AttackMessage.SetActive(false);
                active = false;
            }
            if(attack.isAttacking)
            {
                teachAttack = false;
                if(active)
                {
                    AttackMessage.SetActive(false);
                    active = false;
                }
            }

        }
    }
    public void activateBlock()
    {
        if(teachBlock)
        {
            movement.canTurn = false;
            AttackMessage.SetActive(false);
            BlockMessage.SetActive(true);
            active = true;
            Time.timeScale = 0;

        }

    }

    private float disntaceToEnnemy()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(playerBody.transform.position.x, playerCollider.bounds.center.y), new Vector2(Mathf.Sign(playerBody.transform.localScale.x), 0), 20);
        float distance = Mathf.Infinity;
        foreach(RaycastHit2D i in hits)
        {
            if (i.collider.CompareTag("Enemy") && distance > i.distance)
                distance = i.distance;
        }
        return distance;
    }
}
