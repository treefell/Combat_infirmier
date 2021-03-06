using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerPersistent : MonoBehaviour
{
    private int defaultTimer = 3;
    public static GameManagerPersistent Instance { get; private set; } // static so it can be called from anywhere

    public GameObject BoxerA;
    public GameObject BoxerB;
    public GameObject NurseA;
    public GameObject NurseB;

    public GameObject suddenDeath;
    public Timer timer;

    public int gameTimeMinutes;
    public int gameTimeSeconds;
    public float shelfRefillTime;
    public int healValue;
    public float crateSpawnTime;
    public float scoreIncrementSpeed;
    public int deadBonus = 200;

    [HideInInspector]
    public int scoreA = 0;
    public Text scoreAText;
    [HideInInspector]
    public int scoreB = 0;
    public Text scoreBText;


    [Title("Layers for attacking")]
    public LayerMask friends;
    public LayerMask foes;

    [Title("Hitlag/stun")]
    public float playerHitstunTime = 0.15f;
    public float hitlagTime = 0.05f;
    public float enemyHitstunTime = 0.5f;
    public int bufferingWindow = 10; // number of frames
    public float playerStunTime = 5f;


    [Title("Visu hitboxes")]
    public GameObject circleHitbox;
    public GameObject squareHitbox;

    [Title("Randomizer")]
    public int currentDifficulty;
    public int diffIncrement = 2;

    [Title("EnemiesPerZone")]
    public GameObject[] gameEnemies;


    [HideInInspector]
    public GameObject currentRoom;

    [HideInInspector]
    public List<GameObject> stunnedEnemies;

    [HideInInspector]
    public int deadPlayers = 0;

    public GameObject gameOverScreen;
    private bool gameOver = false;

    public Attack defaultPunch;

    public GameObject SyringeA;
    public GameObject SyringeB;

    public Sprite syringeEmpty;
    public Sprite syringeFull;

    public Vector2 tirednessLevels;

    [HideInInspector]
    public int[] ports = new int[] { 1, 2, 3, 4};

    public GameObject hud;
    
    [System.Serializable]
    public struct SpawnedObjectsProgression
    {
        public SpawnedObject[] spawnedObjects;
    }
    public SpawnedObjectsProgression[] spawnedObjectsProgression;

    private void Awake()
    {
        if (Instance == null) // assigns instance the first time
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // destroys duplicates to keep it unique
        }
    }

    private void Start()
    {
        InitializeVar();
    }

    public void InitializeVar()
    {
        
        scoreA = 0;
        scoreB = 0;
        scoreAText.text = "Score: " + scoreA;
        scoreBText.text = "Score: " + scoreB;
        deadPlayers = 0;
        timer.currentMinutes = gameTimeMinutes;
        timer.currentSeconds = gameTimeSeconds;
        UpdateSyringe();

        
    }
    private void Update()
    {
        Debug.Log(ports[0].ToString() + ports[1].ToString() + ports[2].ToString() + ports[3].ToString());
        if (gameOver)
        {
            if (Input.GetButtonDown("A"))
            {
                SceneManager.LoadScene("Title");
                Reset();
                
                hud.SetActive(false);
            }
        }

    }

    public void GameOver()
    {
        if (scoreA > scoreB)
        {
            gameOverScreen.GetComponentInChildren<Text>().text = "Blue Team Wins!";
        }
        else if (scoreB > scoreA)
        {
            gameOverScreen.GetComponentInChildren<Text>().text = "Red Team Wins!";
        }
        else
        {
            suddenDeath.SetActive(true);
            timer.currentMinutes += 1;
            timer.currentSeconds = 0;
            return;
        }
        gameOverScreen.SetActive(true);
        gameOver = true;
        Time.timeScale = 0;
    }

    private void Reset()
    {
        
        Time.timeScale = 1;
        InitializeVar();
        gameOver = false;
        gameOverScreen.SetActive(false);
        deadPlayers = 0;
    }

    


    public EnemyWave CreateWave()
    {
        EnemyWave wave = new EnemyWave();
        wave.enemies = new List<GameObject>();

        List<GameObject> potentialEnemies = new List<GameObject>();

        int difficultyLeft = currentDifficulty;
        int enemySelected;


        float lowestEnemyDifficulty = Mathf.Infinity;

        foreach (GameObject e in gameEnemies)
        {
            if (e.GetComponent<Fighter>().difficulty < lowestEnemyDifficulty)
            {
                lowestEnemyDifficulty = e.GetComponent<Fighter>().difficulty;
            }
        }


        while (difficultyLeft >= lowestEnemyDifficulty)
        {
            foreach (GameObject enm in gameEnemies)
            {
                if (enm.GetComponent<Fighter>().difficulty <= difficultyLeft)
                {
                    potentialEnemies.Add(enm);
                }
            }

            enemySelected = Random.Range(0, potentialEnemies.Count -1);
            wave.enemies.Add(potentialEnemies[enemySelected]);

            difficultyLeft -= potentialEnemies[enemySelected].GetComponent<Fighter>().difficulty;
        }


        return wave;
    }

    public void RaiseDifficulty()
    {
        currentDifficulty += diffIncrement;
    }


    public void ScoreIncrement(GameObject deadGuy, int score)
    {
        
        if (deadGuy == BoxerB)
        {
            scoreA += score;
        }
        else if(deadGuy == BoxerA)
        {
            scoreB += score;
        }
        scoreAText.text = "Score: "+scoreA;
        scoreBText.text = "Score: " + scoreB;
    }

    public void UpdateSyringe()
    {
        if (NurseA != null)
        {
            if (NurseA.GetComponent<Nurse>().heldItem != null)
            {
                SyringeA.GetComponent<Image>().sprite = NurseA.GetComponent<Nurse>().heldItem.icon;
            }
            else
            {
                SyringeA.GetComponent<Image>().sprite = syringeEmpty;
            }
        }


        if (NurseB != null)
        {
            if (NurseB.GetComponent<Nurse>().heldItem != null)
            {
                SyringeB.GetComponent<Image>().sprite = NurseB.GetComponent<Nurse>().heldItem.icon;
            }
            else
            {
                SyringeB.GetComponent<Image>().sprite = syringeEmpty;
            }
        }
        
    }

    public void SetControlledChar(ControlledChar chara, int port)
    {
        switch (chara)
        {
            case ControlledChar.BoxerA:
                ports[0] = port;
                break;
            case ControlledChar.NurseA:
                ports[1] = port;
                break;
            case ControlledChar.BoxerB:
                ports[2] = port;
                break;
            case ControlledChar.NurseB:
                ports[3] = port;
                break;
            
            
        }
    }

    public void ActivateHud()
    {
        hud.SetActive(true);
    }



}
