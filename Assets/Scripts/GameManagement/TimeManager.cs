using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //Singelton Method : 

    public static TimeManager instantiate;

    private void Awake()
    {
        if(instantiate == null)
        {
            instantiate = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        time = Time.time - elapsedTime;
    }


    public float time, elapsedTime;
}
