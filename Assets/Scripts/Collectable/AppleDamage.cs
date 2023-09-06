using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleDamage : MonoBehaviour
{
    [SerializeField]
    private float appleDamage;

    public AudioClip clip;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Character.ch.takeDamage(appleDamage);
            Character.ch.addScore(-5);
        }

        else if(collision.gameObject.name.Equals("Basket"))
        {
            AudioManager.PlayOneShotSound(clip);
            Character.ch.addScore(10);
        }
    }
}
