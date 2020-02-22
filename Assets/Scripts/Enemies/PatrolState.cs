using UnityEngine;
using System.Collections;

// Klasa odpowiedzialna za stan patrolowania
public class PatrolState : IEnemyAI
{

    EnemyStates enemy;
    int nextWayPoint = 0;
    Vector3[] waypoints;
    bool setPoints;
    int oldDistance;
    float timer;
    
    public PatrolState(EnemyStates enemy)
    {
        this.enemy = enemy;        
        setPoints = false;
        timer = 0;
        oldDistance = 0;
    }

    public void UpdateActions()
    {
        Watch();
        Patrol();
    }

    // Funkcja odpowiadająca za 'widzenie' przeciwnika
    // Gdy przeciwnik zauwazy bohatera przechodzi do stanu gonitwy
    void Watch()
    {           
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);        
        if (dist <= enemy.observeRange){
            Debug.Log("Wyczułem wroga!");
            ToChaseState();
        }         
    }

    // Funkcja odpowiadająca za patrolowanie wzdłuż wyznaczonych punktów
    void Patrol()
    {        
            if(!setPoints) {
                generatePoints();
                setPoints = true;
            }
            enemy.transform.GetComponent<Enemy>().Animation("Walk");        
              
            if(oldDistance != (int) enemy.navMeshAgent.remainingDistance) {
                timer = 0;                
            }

            timer += Time.deltaTime;
            enemy.navMeshAgent.destination = waypoints[nextWayPoint];
            enemy.navMeshAgent.isStopped = false;

            if((enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending) || timer>2)
            {
                int Point = Random.Range(0,4);    
                if(nextWayPoint == Point) {
                    nextWayPoint = (Point + 1) % (waypoints.Length);
                } else {
                    nextWayPoint = Point;
                }
                timer = 0;
            }    
            oldDistance = (int) enemy.navMeshAgent.remainingDistance;
        
    }

    // Funkcja odpowiedzialna za generowanie 4 losowych punktów na mapie
    void generatePoints(){
        // Przypadek dla punktów stworzonych ręcznie
        if(enemy.waypoints.Length != 0){
            waypoints = new Vector3[enemy.waypoints.Length + 1];
            waypoints[enemy.waypoints.Length] = enemy.zeroWayPoint;            
            int i = 0;
            foreach(Transform t in enemy.waypoints){
                waypoints[i] = t.position;                
                i++;                
            }
        // Przypadek dla punktów wygenerowanych    
        } else{
            float x, xs;
            float z, zs;
            waypoints = new Vector3[4];
            waypoints[3] = enemy.zeroWayPoint;

            x = enemy.transform.forward.x * (enemy.patrolRange / 2);
            z = enemy.transform.forward.z * (enemy.patrolRange / 2);
            xs = enemy.zeroWayPoint.x + enemy.transform.forward.x * (enemy.patrolRange / 2); 
            zs = enemy.zeroWayPoint.z + enemy.transform.forward.z * (enemy.patrolRange / 2); 
            
            waypoints[0] = new Vector3(xs + x, enemy.zeroWayPoint.y, zs + z);
            waypoints[1] = new Vector3(xs - z, enemy.zeroWayPoint.y, zs + x);           
            waypoints[2] = new Vector3(xs + z, enemy.zeroWayPoint.y, zs - x);                 
            
        }
    }
    
    public void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.CompareTag("Player"))
        {
            ToAttackState();
        }
        
    }

    public void ToPatrolState()
    {
        Debug.Log("Aktualnie patroluję!");
    }

    public void ToAttackState()
    {        
        enemy.currentState = enemy.attackState;
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