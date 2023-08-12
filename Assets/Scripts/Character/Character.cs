using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    //Singelton design Model : 
    //This model allows us to create a global object of the Character class, this object is accesed from anywhere

    public static Character ch;

    private void Awake()
    {   
        //for the Singelton Design Model : 

        if (ch == null)
        {
            ch = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        //Initial value assignments for character score variable :

        scoreUI = GameObject.Find("Canvas/ScoreText").GetComponent<TMPro.TextMeshProUGUI>();

        //Initial value assignments for character health variables : 

        slider = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();

        slider.value = maxHealth;
        currentHealth = slider.value;
    }

    private void Update()
    {
        SliderUpdate();
        ScoreUIUpdate();
    }

    //Variables and methods for character movement : 

    [SerializeField]
    private float speed;
    private bool isSpeedPot;

    //Getters & Setters: 
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public bool IsSpeedPot
    {
        get { return isSpeedPot; }
        set { isSpeedPot = value; }
    }

    //Variables and methods for character score :

    private float score = 0f;
    private TMPro.TextMeshProUGUI scoreUI;

    public void addScore(float num)
    {
        score += num;
    }

    public void ScoreUIUpdate()
    {
        scoreUI.text = "Score : " + score;
    }

    //Setters
    
    public float Score
    {
        set { score = value; }
    }

    //Variables and methods for character health :  

    private Slider slider;
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    void SliderUpdate()
    {
        slider.value = currentHealth;
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        SliderUpdate();
    }

    //Getters & Setters : 

    public float CurrentHealth
    {
        set { currentHealth = value; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

}