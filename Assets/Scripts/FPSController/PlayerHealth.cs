using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    public AudioClip collectSound;
    public int maxHealth;
    public AudioClip hit;
    public FlashScreen flash;
    AudioSource source;
    public float health;
    public Image Bar;
    bool isGameOver = false;

    void Awake()
    {
        source = GetComponent<AudioSource>();        
    }
    void Start()
    {
        health = maxHealth;        
    }

    void Update() {
        Bar.fillAmount = health / maxHealth;
        if(health <= 0 && !isGameOver)
        {
            isGameOver = true;
            GameManager.Instance.PlayerDeath();
        }
    }

    void EnemyHit(float damage)
    {        
        source.PlayOneShot(this.hit);
        health -= damage;
        Debug.Log("Zostałem trafiony obreżenia:! " + damage + ", pozostało życia: " + health);
        flash.TookDamage();
    }

    void MedkitPickup(){
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        if(health > 0 && health < 100) {
            health = health + 20;
            if (health > 100) {
                health = 100;
            }
        }
        Debug.Log("Aktualny poziom zdrowia: " + health);
    }
}