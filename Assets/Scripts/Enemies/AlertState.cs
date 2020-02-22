using UnityEngine;
using System.Collections;

//Stan zaalarmowania
public class AlertState : IEnemyAI
{
    EnemyStates enemy;
    float timer = 0;

    public AlertState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateActions()
    {
        Search();
        Watch();
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance){
            LookAround();
        }            
    }
    void Watch()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);        
        if (dist <= enemy.patrolRange){
            Debug.Log("Wyczułem wroga!, zaczynam pogoń");
            ToChaseState();
        }
    }

    void LookAround()
    {
        enemy.transform.GetComponent<Enemy>().Animation("Idle");
        enemy.navMeshAgent.isStopped = true;
        timer += Time.deltaTime;
        if(timer >= enemy.stayAlertTime)
        {
            Debug.Log("Wracam do patrolowania!");
            timer = 0;
            ToPatrolState();
        }      
    }
    
    // Funkcja ustawia ostatnie znane miejsce bohatera jako cel
    void Search()
    {
        enemy.navMeshAgent.destination = enemy.lastKnownPosition;
        enemy.navMeshAgent.isStopped = false;
    }

    public void OnTriggerEnter(Collider enemy)
    {

    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToAttackState()
    {
        Debug.Log("Nie powinienem móc tego zrobić!");
    }

    public void ToAlertState()
    {
        Debug.Log("Już jestem w trybie zaalarmowania!");
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
}