using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highScoreText;

    public static MainMenu instant;

    public int highScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HighScore") != null) //using playerprefs to store high score
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }

        instant = this;

        DontDestroyOnLoad(this.gameObject); 
    }

    // Update is called once per frame
    void Update()
    {
        if (highScoreText)
        {
            highScoreText.text = highScore.ToString();
        }
        
    }

    public void changeToGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
