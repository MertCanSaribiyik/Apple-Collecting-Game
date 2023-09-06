using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Variables for collectable prefabs : 
    [SerializeField]
    private GameObject applePrefabs, enemyPrefabs, healthPotPrefab, speedPotPrefab;

    //Variables for where the objects will fall :
    private Vector3 firstPoint, lastPoint;
    private Vector3 basketFirstPoint, basketLastPoint;

    //Variables for Time for objects to fall : 
    private float appleAddingTime = 1.5f, enemyAddingTime = 2.5f, potionAddingTime = 60f;
    private float appleAddingRange = 1f, enemyAddingRange = 2f, potionAddingRange = 60f;
    private float potionType;

    //Variables for meteor rain : 
    [SerializeField]
    private float meteorRainStartTime, meteorRainAddingRange, meteorSpacing, meteorMovement;
    [SerializeField]
    private GameObject background, warningImg;
    private bool isMeteorRainActive = false;
    private float meteorRainType = 10, meteorRainTimeRange = 0.3f;
    private List<float> list = new List<float>();       //We keep a list of meteor rains.
    private byte warningCount = 0;

    //Variable for incrase difficult : 
    private bool isDifficultyIncrased;

    //Variables for game pause processing : 
    [SerializeField]
    private GameObject pauseMenu, resumeButton, buttonText;
    private bool isGamePaused = false;

    //Variable for character dead : 
    private bool isDead;

    private void Awake()
    {
        firstPoint = GameObject.Find("GameManager/FirstPoint").transform.position;
        lastPoint = GameObject.Find("GameManager/LastPoint").transform.position;
        pauseMenu.SetActive(false);
        background.SetActive(false);
        warningImg.SetActive(false);

        //Initial value assignment to drag values of prefabs : 
        //Why are we doing this ?
        //Because mass values change as the difficulty increases, so Mass values are set by default every time the game starts.
        applePrefabs.GetComponent<Rigidbody>().drag = 1.5f;
        enemyPrefabs.GetComponent<Rigidbody>().drag = 0.8f;
        healthPotPrefab.GetComponent<Rigidbody>().drag = 1.8f;
        speedPotPrefab.GetComponent<Rigidbody>().drag = 1.8f;
    }

    private void Update()
    {
        //Game pause :
        GamePause();

        //Incrase Difficult : 
        if (list.Count % 2 == 0 && list.Count != 0 && !isDifficultyIncrased)
            IncraseDifficult();

        //Dead :
        if (!isDead)
            IsDead();

        if (!isMeteorRainActive)
        {
            basketFirstPoint = GameObject.Find("Basket/BasketFirstPoint").transform.position;
            basketLastPoint = GameObject.Find("Basket/BasketLastPoint").transform.position;

            //Adding an apple to the scene : 
            appleAddingTime = AddObject(appleAddingTime, appleAddingRange, applePrefabs);      //Oyun baþladýktan 1 saniye sonra her 1 saniyede bir elma oluþacak.

            //Adding an enemy (meteor) to the scene : 
            enemyAddingTime = AddObject(enemyAddingTime, enemyAddingRange, enemyPrefabs);

            //Adding an potion (health potion or speed potion) to the scene : 
            if (TimeManager.instantiate.time >= potionAddingTime)
            {
                potionType = Mathf.Floor(Random.Range(1f, 3f));     //1 - 2.
            }

            GameObject potion = (potionType == 1) ? healthPotPrefab : speedPotPrefab;


            potionAddingTime = AddObject(potionAddingTime, potionAddingRange, potion);

            //Aþaðýdaki deðiþken asla alamayacaðý bir deðere eþitlenerek meteor yaðmurunun belirli bir süre
            //içinde yalnýzca bir kez oluþmasýna izin verilir. 
            meteorRainType = 10f;
        }


        //Meteor rain : 

        if (TimeManager.instantiate.time >= meteorRainStartTime)
        {

            if (warningCount == 0)
            {
                StartCoroutine(Warning());
                return;
            }

            if (meteorRainType == 10f)
            {
                meteorRainType = Mathf.Floor(Random.Range(1f, 3f));         //1 - 2

                if (list.Count > 1)
                {
                    if (list[list.Count - 1] == 1f && list[list.Count - 2] == 1f)
                    {
                        meteorRainType = 2f;
                    }

                    else if (list[list.Count - 1] == 2f && list[list.Count - 2] == 2f)
                    {
                        meteorRainType = 1f;
                    }
                }
            }

            if (meteorRainType == 1f)
            {
                StartCoroutine(FirstMeteorRain());
            }

            else
            {
                StartCoroutine(SecondMeteorRain());
            }


            meteorRainStartTime = TimeManager.instantiate.time + meteorRainAddingRange;
            list.Add(meteorRainType);
            isDifficultyIncrased = false;
        }
    }

    //Methods for adding objects to the scene : 
    private float AddObject(float nextObjAddingTime, float objAddingRange, GameObject obj)
    {
        if (TimeManager.instantiate.time >= nextObjAddingTime)
        {
            if (basketFirstPoint.x <= firstPoint.x)
                basketFirstPoint.x = firstPoint.x;

            else if (basketLastPoint.x >= lastPoint.x)
                basketLastPoint.x = lastPoint.x;

           Add(obj, RandomXPositionGenerator(basketFirstPoint.x, basketLastPoint.x));

            nextObjAddingTime = TimeManager.instantiate.time + objAddingRange;
        }

        return nextObjAddingTime;
    }

    private GameObject Add(GameObject obj, Vector3 position)
    {
        return Instantiate(obj, position, Quaternion.identity);
    }

    private Vector3 RandomXPositionGenerator(float first, float last)
    {
        float xPosition = Random.Range(first, last);
        return new Vector3(xPosition, this.firstPoint.y, this.firstPoint.z);
    }

    //Methods for meteor rain : 

    private IEnumerator Warning()
    {
        warningImg.SetActive(true);
        yield return new WaitForSeconds(1f);
        isMeteorRainActive = true;
        yield return new WaitForSeconds(0.5f);
        warningImg.SetActive(false);
        warningCount = 1;
    }

    private IEnumerator FirstMeteorRain()
    {
        isMeteorRainActive = true;
        background.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        float random = Mathf.Floor(Random.Range(0, 2));
        float meteorXPos, space = meteorSpacing, move = meteorMovement;

        if (random == 0)         //From left to right
        {
            meteorXPos = firstPoint.x;
        }

        else                     //From right to left. 
        {
            meteorXPos = lastPoint.x;
            space *= -1;
            move *= -1;
        }

        while (true)
        {
            //Create Enemy : 
            GameObject enemy = Add(enemyPrefabs, new Vector3(meteorXPos, this.firstPoint.y, this.firstPoint.z));

            //Enemy movement : 
            enemy.GetComponent<Rigidbody>().velocity = Vector3.right * move;

            meteorXPos += space;
            yield return new WaitForSeconds(0.3f);

            if (random == 0 && meteorXPos >= lastPoint.x)
                break;

            else if (random == 1 && meteorXPos <= firstPoint.x)
                break;

        }

        yield return new WaitForSeconds(1.5f);
        background.SetActive(false);
        isMeteorRainActive = false;
        warningCount = 0;
    }

    private IEnumerator SecondMeteorRain()
    {
        float time = TimeManager.instantiate.time + 5f;

        isMeteorRainActive = true;
        background.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        while (TimeManager.instantiate.time <= time)
        {
            GameObject enemy = Add(enemyPrefabs, RandomXPositionGenerator(firstPoint.x - meteorMovement / 2, lastPoint.x + meteorMovement / 2));
            enemy.GetComponent<Rigidbody>().drag = 0.1f;
            yield return new WaitForSeconds(meteorRainTimeRange);
        }

        yield return new WaitForSeconds(1.5f);
        background.SetActive(false);
        isMeteorRainActive = false;
        warningCount = 0;
    }

    //Methods for incrase difficult : 

    private void IncraseDifficult()
    {
        isDifficultyIncrased = true;

        //Incrase potion adding time : 
        if (potionAddingRange <= 120f)
            potionAddingRange += 5f;

        //Decrease apple and meteo fall time : 
        if (appleAddingRange >= 0.5f)
            appleAddingRange -= 0.1f;

        if (enemyAddingRange >= 1f)
            enemyAddingRange -= 0.1f;

        //Increase meteor and apple speed : 
        if (applePrefabs.GetComponent<Rigidbody>().drag >= 0.8f)
            applePrefabs.GetComponent<Rigidbody>().drag -= 0.1f;

        if (enemyPrefabs.GetComponent<Rigidbody>().drag >= 0.4f)
            enemyPrefabs.GetComponent<Rigidbody>().drag -= 0.1f;

        //Decrease meteor rain time : 
        if (meteorRainAddingRange >= 16f)
            meteorRainAddingRange -= 2f;

        //Decrease the time range for the second meteor rain : 
        if (meteorRainTimeRange >= 0.15f)
            meteorRainTimeRange -= 0.025f;

    }


    //Methods for game pause processing : 
    private void GamePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        if (Character.ch.CurrentHealth <= 0f)
        {
            SceneOperations.ReloadScene();
        }
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneOperations.PreviosScene();     //Returns to the previous scene.
    }

    //Method for character dead : 

    private void IsDead()
    {
        if (Character.ch.CurrentHealth <= 0f)
        {
            isDead = true;
            resumeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(600f, 150f);
            buttonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Play Again";

            if(PlayerPrefs.GetFloat("highScore", 0) < Character.ch.Score)
            {
                PlayerPrefs.SetFloat("highScore", Character.ch.Score);
            }

            Pause();
        }
    }
}