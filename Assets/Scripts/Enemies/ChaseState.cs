using UnityEngine;
using System.Collections;


//Stan gonitwy
public class ChaseState : IEnemyAI
{

    EnemyStates enemy;

    public ChaseState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateActions()
    {
        Chase();
    }
    
    // Funkcja odpowiedzialna za gonienie przeciwnika
    // Jeśli przeciwnik jest wystarczająco blisko przechodzi do stanu atakowania
    void Chase()
    {

        if(enemy.onlyMelee == true){
            enemy.transform.GetComponent<Enemy>().Animation("Fire");    
            enemy.navMeshAgent.speed = 5f;
        }  else {
            enemy.transform.GetComponent<Enemy>().Animation("Walk");
            
        }   
        
        enemy.navMeshAgent.destination = enemy.player.position;
        enemy.navMeshAgent.isStopped = false;

        if(enemy.navMeshAgent.remainingDistance > enemy.observeRange){
            enemy.lastKnownPosition = enemy.player.position;
            ToAlertState();
        }

        if(enemy.navMeshAgent.remainingDistance <= enemy.attackRange && enemy.onlyMelee == true)
        {            
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
        } else if (enemy.navMeshAgent.remainingDistance <= enemy.shootRange && enemy.onlyMelee == false)
        {                        
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
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
        Debug.Log("Zaczynam atakować gracza!");
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {
        Debug.Log("Już go gonie!");
    }

     public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }
}