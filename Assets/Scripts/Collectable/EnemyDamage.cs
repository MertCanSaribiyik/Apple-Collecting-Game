using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private float enemyDamage;

    public AudioClip clip;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Equals("Basket"))
        {
            AudioManager.PlayOneShotSound(clip);
            Character.ch.takeDamage(enemyDamage);
            Character.ch.addScore(-10);
        }
    }
}
