using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Basket"))
        {
            Character.ch.CurrentHealth = Character.ch.MaxHealth;
        }
    }
}
