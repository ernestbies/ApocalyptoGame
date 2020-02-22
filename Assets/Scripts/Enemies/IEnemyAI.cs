using UnityEngine;
using System.Collections;

// Interfejs opisujący każdy stan
public interface IEnemyAI
{
    void UpdateActions();

    void OnTriggerEnter(Collider enemy);

    void ToPatrolState();

    void ToAttackState();

    void ToAlertState();

    void ToChaseState();
}