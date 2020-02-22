using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Skrypt do obsługi przeciwnika
public class Enemy : MonoBehaviour
{
    int level;
    public int maxHealth;
    public bool isHavePowerCell; 
    float health;
    public GameObject healthBar;
    public GameObject powerCell;
    EnemyStates es;
    NavMeshAgent nma;
    BoxCollider bc;
    [HideInInspector]
    public Animator animator;
    string currentanime;
    float CenterY;
    float SizeY;
    GameObject hb;
    bool visibleHealthBar;
    Vector3 position;
    public bool isDie;

    
    void Awake()
    {
        es = GetComponent<EnemyStates>();
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();
        this.currentanime="Idle";
        level = GameManager.level;
        setEnemyHealth(level);
        CenterY = bc.center.y;
        SizeY = bc.size.y;
        visibleHealthBar = false;                                
        isDie = false;        
    }

    private void Update()
    {
        if(!isDie){
            if (health <= 0)
            {
                es.enabled = false;
                nma.enabled = false;
                Destroy(hb);
                isDie = true;
                if(isHavePowerCell==true){
                    position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1 );
                    //Utworzenie ogniwa osłony na danej pozycji
                    powerCell.name = "powerCell ("+transform.name+")";              
                    GameObject.Instantiate(powerCell, position, Quaternion.identity);
                }                
            } else {
                if (visibleHealthBar==true){
                    hb.GetComponent<HealthBarCamera>().fill = (health / maxHealth);       
                    hb.transform.position = new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y*2, transform.position.z);     
                }                
            }        
        } else {
                if(transform.name.Contains("Boss")){
                    Animation("Die");   
                } else {      
                    Animation("Idle");                
                }    
        }
        
    } 
    
    // Funkcja, która uruchamia się po trafieniu przez gracza
    void PistolHit(int damage)
    {
        if(!isDie) {
            if (health == maxHealth) {
                //Stworzenie Health Bara
                hb = GameObject.Instantiate(healthBar, new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y*2, transform.position.z), Quaternion.identity);                          
                visibleHealthBar = true;
            }
            health = health - damage;
            Debug.Log("Ja "+transform.name + " otrzymalem obrazenia: " + damage + ", maksymalne zycie: " + maxHealth) ;
        }        
    }

    
    // Funkcja do odtwarzania animacji
    public void Animation(string anime){
        if (this.currentanime != anime){            
            Debug.Log(transform.name + ", poprzednia animacja: " + this.currentanime + ", aktualna animacja: " + anime);
            resetAnimations();
            animator.SetBool (this.currentanime+"To"+anime, true);
            this.currentanime = anime; 
            if(this.currentanime == "Fire" && transform.name.Contains("Robot_red")) { // zmniejszenie Box Collidera 
                bc.center = new Vector3(bc.center.x, CenterY/2, bc.center.z);
                bc.size = new Vector3(bc.size.x, SizeY/2, bc.size.z);
            } else {
                bc.center = new Vector3(bc.center.x, CenterY, bc.center.z);
                bc.size = new Vector3(bc.size.x, SizeY, bc.size.z);                
            }
        }
        
    }   

    // Wyłącznie wszystkich animacji
    void resetAnimations(){
        animator.SetBool ("IdleToWalk", false);
        animator.SetBool ("WalkToIdle", false);
        animator.SetBool ("WalkToFire", false);
        animator.SetBool ("FireToWalk", false);
        animator.SetBool ("FireToIdle", false);
        if(transform.name == "Boss"){
            animator.SetBool ("FireToDie", false);
            animator.SetBool ("WalkToDie", false);
        }
    }

    // Ustawienie życia przeciwników w zależnosci od poziomu trudności
    public void setEnemyHealth(int level) {
        if(level == 0) {
            if(transform.name.Contains("Robot")){
                maxHealth = 60;
            }
            if(transform.name.Contains("Rhino")){
                maxHealth = 30;
            }
            if(transform.name.Contains("Boss")){
                maxHealth = 200;
            }
        }else if(level == 1) {
            if(transform.name.Contains("Robot")){
                maxHealth = 80;
            }
            if(transform.name.Contains("Rhino")){
                maxHealth = 50;
            }
            if(transform.name.Contains("Boss")){
                maxHealth = 300;
            }
        }else {
            if(transform.name.Contains("Robot")){
                maxHealth = 100;
            }
            if(transform.name.Contains("Rhino")){
                maxHealth = 80;
            }
            if(transform.name.Contains("Boss")){
                maxHealth = 500;
            }
        }
        health = maxHealth;
    }

}