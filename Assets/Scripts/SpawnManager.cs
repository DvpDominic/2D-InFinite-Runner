using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject levelPart_prefab; //level part prefab
    public GameObject firstPart; //the current level part in which the game starts

    public List<GameObject> pooledLevelParts; //list for object pooling of level parts

    public static SpawnManager instant;

    // Start is called before the first frame update
    void Start()
    {
        instant = this;
        pooledLevelParts = new List<GameObject>();

        for(int i = 0; i < 2; i++) //instantiating level parts and adding them to the list
        {
            GameObject levelPart = (GameObject)Instantiate(levelPart_prefab);
            levelPart.SetActive(false);
            pooledLevelParts.Add(levelPart);
        }

        pooledLevelParts.Add(firstPart);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getPooledLevel() //getting the pooled level part for use
    {
        for(int i = 0; i < pooledLevelParts.Count; i++)
        {
            if (!pooledLevelParts[i].activeInHierarchy) //if they are disabled, we will enable them and use them
            {
                return pooledLevelParts[i];
            }
        }
        return null;
    }


    public void removeOthers() //disabling all level parts other than current level part
    {
        for (int i = 0; i < pooledLevelParts.Count; i++)
        {
            if (!pooledLevelParts[i].GetComponent<LevelManager>().isCurrent)
            {
                pooledLevelParts[i].SetActive(false);
                pooledLevelParts[i].GetComponent<LevelManager>().coinSpawned = false; //this will be reset the coin spawn boolean
                pooledLevelParts[i].GetComponent<LevelManager>().disbleCoins(); //disabling coins which are active in their poollist
                pooledLevelParts[i].GetComponent<LevelManager>().disableEnemies(); // disabling all the enemies on the level part
            }
        }
    }

}
