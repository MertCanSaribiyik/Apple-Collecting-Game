using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Equals("Basket"))
        {
            Character.ch.IsSpeedPot = true;
        }

    }

}
