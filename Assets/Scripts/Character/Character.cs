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
        }

        //Initial value assignments for character score variable :

        scoreUI = GameObject.Find("Canvas/ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        highScoreUI = GameObject.Find("Canvas/HighScoreText").GetComponent<TMPro.TextMeshProUGUI>();

        highScoreUI.text = "High Score : " + PlayerPrefs.GetFloat("highScore", 0).ToString();

        //Initial value assignments for character health variables : 

        healthSlider = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();

        healthSlider.value = maxHealth;
        currentHealth = healthSlider.value;
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
    private TMPro.TextMeshProUGUI highScoreUI;

    public void addScore(float num)
    {
        score += num;
        ScoreUIUpdate();
    }

    public void ScoreUIUpdate()
    {
        scoreUI.text = "Score : " + score;
    }

    //Setters
    
    public float Score
    {
        set { score = value; }
        get { return score; }
    }

    //Variables and methods for character health :  

    private Slider healthSlider;
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    public void HealthSliderUpdate()
    {
        healthSlider.value = currentHealth;
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        HealthSliderUpdate();
    }

    //Getters & Setters : 

    public float CurrentHealth
    {
        set { currentHealth = value; }
        get { return currentHealth; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

}