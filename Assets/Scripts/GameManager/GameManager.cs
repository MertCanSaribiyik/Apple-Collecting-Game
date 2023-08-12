using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Variables for collectable prefabs : 
    [SerializeField]
    private GameObject applePrefabs, enemyPrefabs, healthPotPrefab, speedPotPrefab;

    //Variables for where the objects will fall :
    private Vector3 firstPoint, lastPoint;
    private Vector3 basketFirstPoint, basketLastPoint;

    //Variables for Time for objects to fall : 
    private float appleAddingTime = 1f, enemyAddingTime = 2f, heathPotAddingTime = 60f, speedPotAddingTime = 45f;

    //Variables for meteor rain : 
    [SerializeField]
    private float meteorRainStartTime, meteorFallRange, meteorSpacing, meteorMovement;
    [SerializeField]
    private GameObject background, warningImg;
    private bool isMeteorRainActive = false;
    private float meteorRainType = 10;
    private List<float> List = new List<float>();
    private byte warningCount = 0;

    //Variables for game pause processing : 
    [SerializeField]
    private GameObject pauseMenu;
    private bool isGamePaused = false;

    private void Awake()
    {
        firstPoint = GameObject.Find("GameManager/FirstPoint").transform.position;
        lastPoint = GameObject.Find("GameManager/LastPoint").transform.position;
        pauseMenu.SetActive(false);
        background.SetActive(false);
        warningImg.SetActive(false);
    }
    private void Update()
    {
        //Game pause :
        GamePause();

        if (!isMeteorRainActive)
        {
            basketFirstPoint = GameObject.Find("Basket/BasketFirstPoint").transform.position;
            basketLastPoint = GameObject.Find("Basket/BasketLastPoint").transform.position;

            appleAddingTime = AddObject(appleAddingTime, 1f, applePrefabs);      //Oyun baþladýktan 1 saniye sonra her 1 saniyede bir elma oluþacak.

            enemyAddingTime = AddObject(enemyAddingTime, 2f, enemyPrefabs);

            heathPotAddingTime = AddObject(heathPotAddingTime, 60f, healthPotPrefab);

            speedPotAddingTime = AddObject(speedPotAddingTime, 45f, speedPotPrefab);

            meteorRainType = 10f;
        }


        //Meteor rain : 

        if (Time.time >= meteorRainStartTime)
        {
            
            if(warningCount == 0)
            {
                StartCoroutine(Warning());
                return;
            }

            if (meteorRainType == 10f)
            {
                meteorRainType = Mathf.Floor(Random.Range(1f, 3f));

                if (List.Count > 1)
                {
                    if(List[List.Count - 1] == 1f && List[List.Count - 2] == 1f)
                    {
                        meteorRainType = 2f;
                    }

                    else if(List[List.Count - 1] == 2f && List[List.Count - 2] == 2f)
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


            meteorRainStartTime = Time.time + meteorFallRange;
            List.Add(meteorRainType);
        }
    }

    //Methods for adding objects to the scene : 
    private float AddObject(float nextObjAddingTime, float objAddingRange, GameObject obj)
    {
        if (Time.time >= nextObjAddingTime)        
        {
            if (basketFirstPoint.x <= firstPoint.x)
            {
                basketLastPoint.x += Mathf.Abs(firstPoint.x - basketFirstPoint.x);
                basketFirstPoint.x = firstPoint.x;
            }

            else if (basketLastPoint.x >= lastPoint.x)
            {
                basketFirstPoint.x -= Mathf.Abs(lastPoint.x - basketLastPoint.x);
                basketLastPoint.x = lastPoint.x;
            }

            Add(obj, RandomXPositionGenerator(basketFirstPoint.x, basketLastPoint.x));
            nextObjAddingTime = Time.time + objAddingRange;
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
        isMeteorRainActive = true;
        background.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 20; i++)
        {
            GameObject enemy = Add(enemyPrefabs, RandomXPositionGenerator(firstPoint.x - meteorMovement / 2, lastPoint.x + meteorMovement / 2));
            enemy.GetComponent<Rigidbody>().drag = 0.2f;
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1.5f);
        background.SetActive(false);
        isMeteorRainActive = false;
        warningCount = 0;
    }

    //Methods for game pause processing : 
    private void GamePause()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
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
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);   //Returns to the previous scene.
    }

}