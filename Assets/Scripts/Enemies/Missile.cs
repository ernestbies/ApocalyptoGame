using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasa odpowiedzialna za kule ogniste
public class Missile : MonoBehaviour
{
    [HideInInspector]
    public float damage; //obrażenia od kuli
    [HideInInspector]
    public float speed; //szybkość kuli
    Transform player; //obiekt "Player"
    int missileLife; //czas życia kuli
    float timer; //odliczanie czasu

    void Start()
    {
        missileLife = 15;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(player);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > missileLife)
            Destroy(this.gameObject);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("EnemyHit", damage, SendMessageOptions.DontRequireReceiver);
        }
        Destroy(this.gameObject);
    }
}