using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private float enemyDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Equals("Basket"))
        {
            Character.ch.takeDamage(enemyDamage);
            Character.ch.addScore(-10);
        }
    }
}
