using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private TrailRenderer trailEffect;

    private float firstPoint, lastPoint;
    private bool takingSpeedPot;

    private void Awake()
    {
        firstPoint = GameObject.Find("FirstPoint").transform.position.x - 1f;
        lastPoint = GameObject.Find("LastPoint").transform.position.x + 1f;
        trailEffect = GetComponent<TrailRenderer>();
        trailEffect.enabled = false;
    }

    private void Update()
    {
        if (Character.ch.IsSpeedPot)
            StartCoroutine(IncraseSpeed());
    }

    private void FixedUpdate()
    {
        Move();

        if(transform.position.x <= firstPoint)
        {
            StartCoroutine(StopStartSpeedEffect()); 
            transform.position = new Vector3(lastPoint, transform.position.y, transform.position.z);
        }

        else if(transform.position.x >= lastPoint)
        {
            StartCoroutine(StopStartSpeedEffect());
            transform.position = new Vector3(firstPoint, transform.position.y, transform.position.z);
        }

    }

    private void Move()
    {
        //Move Horizontal : 
        float horizontalMove = Input.GetAxis("Horizontal") * Character.ch.Speed;
        transform.Translate(horizontalMove * Time.deltaTime, 0, 0);
    }

    private IEnumerator IncraseSpeed()
    {
        Character.ch.IsSpeedPot = false;
        takingSpeedPot = true;
        trailEffect.enabled = true;

        Character.ch.Speed *= 2;
        yield return new WaitForSeconds(5f);
        Character.ch.Speed /= 2;

        takingSpeedPot = false;
        trailEffect.enabled = false;
    }

    IEnumerator StopStartSpeedEffect()
    {
        if(takingSpeedPot)
        {
            trailEffect.enabled = false;
            yield return new WaitForSeconds(0.2f);
            trailEffect.enabled = true;
        }
    }
}
