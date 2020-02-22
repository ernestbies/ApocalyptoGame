using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Inventory : MonoBehaviour
{
    public AudioClip collectSound;
    public AudioClip explosionSound;

    public bool[] chargeTable;
    int charge = 0;
    GameObject pistol;
    PlayerHealth playerHealth;

    // HUD
    public Texture2D[] hudCharge;
    public RawImage chargeHudGUI;

    // Generator
    public Texture2D[] meterCharge;
    public Renderer meter;

    // Messages
    Messages message;

    public GameObject explosive;

    GameObject motherShip;

    ParticleSystem pS;

    Transform mainCamera;

    // Cheats
    public bool isCheatsEnabled = false;

    float endTimer;
    bool gameOver;

    int steps;

    // Start is called before the first frame update
    void Start()
    {
        charge = 0;
        endTimer = 0.0f;
        gameOver = false;
        steps = 0;
        chargeTable = new bool[4];
        message = GetComponent<Messages>();
        pistol = GameObject.FindGameObjectWithTag("Pistol").gameObject;
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        pS = explosive.GetComponent<ParticleSystem>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        Cheats();
        if(gameOver){
            endGame();
        }
    }

    void CellPickup(string name){
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        charge++;
        Debug.Log("Podniosłem " + name);
        if (name.Contains("powerCell (Robot_red)")){
            chargeTable[0] = true;
        }
        if (name.Contains("powerCell (Ground)")){
            chargeTable[1] = true;
        }
        if (name.Contains("powerCell (Rhino_PBR)")){
            chargeTable[2] = true;
        }
        if (name.Contains("powerCell (Boss)")){
            chargeTable[3] = true;
        }
        int i = 0;
        while(i<4 && chargeTable[i]){
            i++;
        }

        message.nextNumber(i);
        chargeHudGUI.texture = hudCharge[charge];
    }

     void AmmoPickup(){
        int ammo = pistol.GetComponent<Pistol>().ammoLeft;
        ammo = ammo + 12;
        pistol.GetComponent<Pistol>().ammoLeft = ammo;
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }

    void LoadGenerator(){        
        meter.material.mainTexture = meterCharge[charge];

        if (charge == 4) {
            Debug.Log("Uruchomiono animację wybuchu i zniszczenia statku!!");
            //endGame();
            gameOver = true;
            endTimer = 0.0f;
        } else {
            message.nextNumber(9);
        }
    }

    void endGame()
    {          
       Quaternion neededRotation = Quaternion.LookRotation(motherShip.transform.position - mainCamera.position);
       mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, neededRotation, endTimer);

       if (endTimer > 2 && steps!= 2){
          steps = 2; 
          pS.Play(); 
          AudioSource.PlayClipAtPoint(explosionSound, transform.position);
       } 
       if (endTimer > 3 && steps!= 3){
          steps = 3; 
          Destroy(motherShip);
          GameManager.Instance.PlayerWin();
          gameOver = false;    
       }
       if (endTimer < 6) {
           endTimer += Time.deltaTime;
       }
    }

    void Cheats() {
        if(isCheatsEnabled==true){
            if(Input.GetKeyDown("z")) {
                pistol.GetComponent<Pistol>().ammoLeft = 99;
                playerHealth.health = 1000;
                Debug.Log("Doładowanie amunicji oraz życia!!");
                
                //charge  = 4;                
                //gameOver = true;
            }
        }

        /*
        if(Input.GetKeyDown("e")) {
            Debug.Log("Uruchomiono animację wybuchu i zniszczenia statku!!");
            pS.Play();  
            Destroy(motherShip);   
        }*/           
        
    }
}
