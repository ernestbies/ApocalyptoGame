using UnityEngine;
using System.Collections;

//Klasa odpowiedzialna za stan Ataku
public class AttackState : IEnemyAI
{
    EnemyStates enemy;
    float timer;
    Vector3 position;

    public AttackState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateActions()
    {
        if (!GameManager.Instance.isPlayerDeath()){
        
            enemy.transform.LookAt(enemy.player);
            timer += Time.deltaTime;
            float distance = Vector3.Distance(enemy.player.position, enemy.transform.position);
            if (distance > enemy.attackRange && enemy.onlyMelee == true)
            {
                ToChaseState();
            }
            if (distance > enemy.shootRange && enemy.onlyMelee == false)
            {
                ToChaseState();
            }
            if (distance <= enemy.shootRange && distance > enemy.attackRange && enemy.onlyMelee == false && timer >= enemy.attackDelay)
            {
                Attack(true);
                timer = 0;
            }
            if (distance <= enemy.attackRange && timer >= enemy.attackDelay)
            {            
                Attack(false);
                timer = 0;
            }
        }
    }

    void Attack(bool shoot)
    {
        if (shoot == false)        
        {
            Debug.Log("Zaczynam uderzać przeciwnika!");
            enemy.player.SendMessage("EnemyHit", enemy.meleeDamage, SendMessageOptions.DontRequireReceiver);            
        }
        else if (shoot == true)
        {
            enemy.transform.GetComponent<Enemy>().Animation("Fire");
            Debug.Log("Zaczynam strzelać do przeciwnika!");
            if(enemy.transform.name.Contains("Robot_red")){
                position = new Vector3(enemy.transform.position.x - 0.5f, enemy.transform.position.y + 2.0f, enemy.transform.position.z);
            }else{
                position = new Vector3(enemy.transform.position.x - 0.5f, enemy.transform.position.y + 5.0f, enemy.transform.position.z); 
            }    
            GameObject missile = GameObject.Instantiate(enemy.missile, position, Quaternion.identity);
            missile.GetComponent<Missile>().speed = enemy.missileSpeed;
            missile.GetComponent<Missile>().damage = enemy.missileDamage;
        }
    }
   

    public void OnTriggerEnter(Collider enemy)
    {

    }

    public void ToPatrolState()
    {
        Debug.Log("Nie powinienem móc tego zrobić!");
    }

    public void ToAttackState()
    {
        Debug.Log("Nie powinienem móc tego zrobić!");
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
}