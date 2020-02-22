using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    private static bool playerDeath;
    public static int level;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private GameObject deathScreen;
    private GameObject winScreen;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        } else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        playerDeath = false;
        level = 0;
        InitGame();
    }

    private void Start()
    {
        deathScreen = transform.Find("DeathScreen").gameObject;
        winScreen = transform.Find("WinScreen").gameObject;
    }

    void Update() {
    }

    void InitGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayerDeath()
    {
        playerDeath = true;
        deathScreen.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player").gameObject;
        GameObject pistol = GameObject.FindGameObjectWithTag("Pistol").gameObject;
        player.GetComponent<CharController_Motor>().enabled = false;
        player.GetComponent<PlayerHealth>().enabled = false;
        pistol.GetComponent<Pistol>().enabled = false;

        foreach (Transform child in player.transform)
        {
            if (child.tag != "MainCamera")
                child.gameObject.SetActive(false);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.tag = "Untagged";
    }

    public void PlayerWin(){

        playerDeath = true;
        winScreen.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player").gameObject;
        GameObject pistol = GameObject.FindGameObjectWithTag("Pistol").gameObject;
        player.GetComponent<CharController_Motor>().enabled = false;
        player.GetComponent<PlayerHealth>().enabled = false;
        pistol.GetComponent<Pistol>().enabled = false;

        foreach (Transform child in player.transform)
        {
            if (child.tag != "MainCamera")
                child.gameObject.SetActive(false);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.tag = "Untagged";
    }

    public bool isPlayerDeath(){
        return playerDeath;
    }

}
