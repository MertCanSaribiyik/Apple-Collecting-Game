using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyCollectable : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Basket") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }
}
