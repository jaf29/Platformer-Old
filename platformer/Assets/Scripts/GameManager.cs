using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject[] spawnPoints;
    public GameObject[] spawnPoints2;
    public GameObject dragon; //link to the prefab
    public GameObject bat;
    
    public bool playerDied;
    public static GameManager instance = null;
    public Text scoreText;
    public Text winText;

    private GameObject[] dragons;
    private GameObject[] bats;
    private int crystalCount = 0;
	void Awake () {
        // the following logic follows our singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this) //someone it creating another game manager... not allowed!
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // create the enemies when the game starts
        SpawnEnemy();
        SetScoreText();
        winText.text = "";
    }

    void Update()
    {
        // make more enemies if there are none
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        // find where the enemies should spawn
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        spawnPoints2 = GameObject.FindGameObjectsWithTag("SpawnPoints2");
        dragons = new GameObject[spawnPoints.Length];
        bats = new GameObject[spawnPoints2.Length];

        // walk through the spawn points and create an enemy at each point
        int i = 0;
        int j = 0;
        foreach(GameObject spawnPoint in spawnPoints) { 
            dragons[i++]=Instantiate(dragon, spawnPoint.transform);
        }
        
        foreach(GameObject spawnPoint2 in spawnPoints2)
        {
            bats[j++] = Instantiate(bat, spawnPoint2.transform);
        }
    }

    //increase the number of crystals picked up
    public void IncrementCrystalCount()
    {
        crystalCount++;
        SetScoreText();
        //Debug.Log(crystalCount);
        if (crystalCount == 10)
        {
            winText.text = "You Win!";
            PlayerDeath();
        }
    }

    // control all the events that happen when a player dies
    public void PlayerDeath()
    {
        playerDied = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) // if player still exists
        {
            player.GetComponent<PlayerController>().Die();
            player.GetComponent<AudioSource>().Play();
        }

        // make each dragon respond to death
        foreach (GameObject aDragon in dragons)
            if(aDragon != null) //make sure dragon has not been destroyed
                aDragon.SendMessage("PlayerDeath");

        // restarts the game in 3 seconds -- provides time for sounds and animation before restarting
        crystalCount = 0;
        Invoke("RestartGame", 3f);

    }

    private void RestartGame()
    {
        //restart scene
        SceneManager.LoadScene(0);
        //reset player status so the game can resume
        playerDied = false;
    }

    private void SetScoreText()
    {
        scoreText.text = "Score: " + crystalCount.ToString("0");
    }
}
