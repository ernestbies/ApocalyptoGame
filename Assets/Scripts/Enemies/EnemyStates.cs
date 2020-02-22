using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyStates : MonoBehaviour
{
    public Transform[] waypoints;
    public int observeRange;
    public int patrolRange;
    public int attackRange;
    public int shootRange;
    public GameObject missile;
    public float missileDamage;
    public float missileSpeed;
    public bool onlyMelee = false;
    public float meleeDamage;
    public float attackDelay;
    public LayerMask raycastMask;
    public float stayAlertTime;

    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public AttackState attackState;
    [HideInInspector]
    public ChaseState chaseState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public IEnemyAI currentState;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Vector3 lastKnownPosition;
    [HideInInspector]
    public Vector3 zeroWayPoint; // początkowa pozycja
    [HideInInspector]
    public Transform player;

        
    void Awake()
    {
        // Tworzymy instancje każdego ze stanu i przekazujemy do nich obiekt EnemyStates
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        chaseState = new ChaseState(this);
        patrolState = new PatrolState(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        zeroWayPoint = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        currentState = patrolState;           
    }

    void Update()
    {
        currentState.UpdateActions();
    }

    void OnTriggerEnter(Collider otherObj)
    {
        // Po wejściu w interakcje z innym obiektem wywołujemy funkcję OnTriggerEnter zgodną z aktualnym stanem
        currentState.OnTriggerEnter(otherObj);
    }    

    // Przypadek dla dalekich strzałów (w momencie oddania strzału)
    void HiddenShot(Vector3 shotPosition)
    {
        Debug.Log("Player strzelił z daleka!!");
        lastKnownPosition = shotPosition;
        currentState = alertState;

        //Przypadek dla Boss-a
        if(transform.name.Contains("Boss")){
            observeRange = 100;
            shootRange = 100;
            currentState = attackState;
        }
    }        

}