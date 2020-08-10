using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void changeToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void changeToGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

}
