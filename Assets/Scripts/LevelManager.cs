using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayerController player;

    public Transform left;
    public Transform right;

    public Transform leftSpawner; // if player goes to left from this point, a level part will be activated on left side 
    public Transform rightSpawner; // if player goes to right from this point, a level part will be activated on right side

    public bool spawnedLeft = false; //if there is level part on left side of current part
    public bool spawnedRight = false; //if there is level part on right side of current part
    public bool otherDc = true; //if all other level parts are disabled

    public bool isCurrent; //if this level part is current, by current we mean that player is on the level part
    public bool coinSpawned; // if coins are spawned or can be spawned on level part
    public bool canSpawnEnemies; // if we can spawn enemies on level part

    public List<GameObject> enemyList; //list of enemies
    public GameObject enemyPrefab;

    public List<GameObject> pooledCoins; //pool list of coins
    public GameObject coinPrefab;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        pooledCoins = new List<GameObject>();
        for(int i = 0; i < 15; i++) //instantiating coins and adding them to pool list
        {   
            GameObject coin = (GameObject)Instantiate(coinPrefab);
            coin.SetActive(false);
            pooledCoins.Add(coin);
        }
        coinSpawned = false;

        enemyList = new List<GameObject>();
        for(int i = 0; i < 3; i++) //instantiating enemies and adding them to enemy list
        {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyList.Add(enemy);
        }

        canSpawnEnemies = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > left.position.x && player.transform.position.x < right.position.x) //to check if the player is on current level part or not
        {
            isCurrent = true;
        }
        else
        {
            isCurrent = false;
        }


        if (isCurrent)
        {
            if (player.transform.position.x < leftSpawner.position.x)
            {
                spawnLeft(); //if this level part is current and player goes to left side, we will pool a level part at left
                otherDc = false;
            }
            else if (player.transform.position.x > rightSpawner.position.x)
            {
                spawnRight(); //if this level part is current and player goes to right side, we will pool a level part at right
                otherDc = false;
            }
            else
            {   //if the player is in between the current part, we will not pool any other level parts and will disable all other level parts 
                spawnedLeft = false;
                spawnedRight = false;
                deactiveOther();
                enableCoins(); //enabling the coins
                enableEnemies(); //enabling the enemies
            }
        }
    }

    public void spawnLeft() //to pool a level part at left side
    {
        if (!spawnedLeft)
        {
            GameObject newLevelObj = SpawnManager.instant.getPooledLevel();
            newLevelObj.transform.position = new Vector3(this.transform.position.x - 246.7f, transform.position.y, transform.position.z);
            newLevelObj.SetActive(true);
            spawnedLeft = true;
            newLevelObj.GetComponent<LevelManager>().spawnedRight = true; //to check if there is any level part on the right side
            newLevelObj.GetComponent<LevelManager>().coinSpawned = true; //this will prevent activating coins of left side
            newLevelObj.GetComponent<LevelManager>().canSpawnEnemies = false; //this will prevent activating enemies at left side
        }
        
    }

    public void spawnRight()
    {
        if (!spawnedRight)
        {
            GameObject newLevelObj = SpawnManager.instant.getPooledLevel();
            newLevelObj.transform.position = new Vector3(this.transform.position.x + 246.7f, transform.position.y, transform.position.z);
            newLevelObj.SetActive(true);
            spawnedRight = true;
            newLevelObj.GetComponent<LevelManager>().spawnedLeft = true; //to check if there is any level part on left side
            newLevelObj.GetComponent<LevelManager>().coinSpawned = false; // this will indicate that we can activate coins on that level part
            newLevelObj.GetComponent<LevelManager>().canSpawnEnemies = true; // this will indicate that we can actictivate enemies on that level part
        }
        
    }

    public void deactiveOther() //deactivating other level parts
    {
        if (!otherDc && isCurrent)
        {
            SpawnManager.instant.removeOthers();
            otherDc = true;
        }
    }

    public void enableCoins() //enabling the coins on the current level part
    {
        if (!coinSpawned)
        {
            float tempPosX = player.transform.position.x;
            for (int i = 0; i < pooledCoins.Count; i++)
            {
                pooledCoins[i].transform.position = new Vector3(Random.Range(tempPosX + 3, tempPosX + 10), Random.Range(-6f, 0), 0); //this will prevent overlapping of the coins
                tempPosX = pooledCoins[i].transform.position.x;
                pooledCoins[i].SetActive(true);
            }
            coinSpawned = true; //indicates that coins are activated one time
        }
    }

    public void disbleCoins()
    {
        for(int i = 0; i < pooledCoins.Count; i++)
        {
            if (pooledCoins[i].activeInHierarchy)
            {
                pooledCoins[i].SetActive(false);
            }
        }
    }

    public void enableEnemies()
    {
        if (canSpawnEnemies)
        {
            float tempPosX = player.transform.position.x;
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].transform.position = new Vector3(Random.Range(tempPosX + 30, tempPosX + 75), -5.9f , 0); //this will prevent overlapping of enemies
                tempPosX = enemyList[i].transform.position.x;
                enemyList[i].SetActive(true);
                canSpawnEnemies = false; //indicates that enemies are activated 
            }
        }
    }

    public void disableEnemies()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].SetActive(false);
        }
    }

}
